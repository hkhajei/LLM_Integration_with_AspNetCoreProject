using Microsoft.AspNetCore.Mvc;
using AspNetCoreProject.Models.Services; // Ensure this is present
using Microsoft.SemanticKernel; // For Kernel
using Microsoft.SemanticKernel.ChatCompletion; // For IChatCompletionService, ChatHistory
using Microsoft.SemanticKernel.Embeddings; // For IEmbeddingService
using Microsoft.SemanticKernel.Connectors.Ollama; // For OllamaEmbeddingClient (if you decide to use it directly)

namespace AspNetCoreProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        // Use IChatCompletionService directly if you want simpler chat interactions
        // Or IChatClient if you prefer the abstraction from Microsoft.Extensions.AI
        private readonly IChatCompletionService _chatCompletionService;
        private readonly ITextEmbeddingGenerationService _embeddingService; // For Q&A
        private readonly DocumentService _documentService; // For Q&A

        // Inject IChatCompletionService, IEmbeddingService, and your DocumentService
        public ChatController(IChatCompletionService chatCompletionService, ITextEmbeddingGenerationService embeddingService, DocumentService documentService)
        {
            _chatCompletionService = chatCompletionService;
            _embeddingService = embeddingService;
            _documentService = documentService;
        }

        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] ChatRequestModel request)
        {
            var chatHistory = new ChatHistory();
            chatHistory.AddUserMessage(request.Message);

            // GetChatMessageContentsAsync is common for IChatCompletionService
            // It returns a list of ChatMessageContent, usually you take the first one.
            var result = await _chatCompletionService.GetChatMessageContentsAsync(chatHistory);
            return Ok(new { Response = result.FirstOrDefault()?.Content });
        }

        [HttpPost("qa")]
        public async Task<IActionResult> QaOverDocuments([FromBody] ChatRequestModel request)
        {
            // 1. Generate embedding for the user's question
            // GenerateEmbeddingsAsync now takes a list and returns a list
            var queryEmbeddings = await _embeddingService.GenerateEmbeddingsAsync(new List<string> { request.Message });
            var queryEmbedding = queryEmbeddings[0].ToArray();

            // 2. Retrieve relevant document chunks
            var relevantChunks = _documentService.RetrieveRelevantChunks(queryEmbedding, 3); // Get top 3

            // 3. Construct the prompt with context
            var context = string.Join("\n\n", relevantChunks);
            var prompt = $"Answer the following question based on the provided context. If the answer is not in the context, state that you don't know.\n\nContext:\n{context}\n\nQuestion: {request.Message}";

            var chatHistory = new ChatHistory();
            chatHistory.AddSystemMessage("You are a helpful assistant that answers questions based on provided context.");
            chatHistory.AddUserMessage(prompt);

            // 4. Get the LLM's response
            var result = await _chatCompletionService.GetChatMessageContentsAsync(chatHistory);
            return Ok(new { Response = result.FirstOrDefault()?.Content });
        }

        // This method is no longer needed here as embedding generation is handled by DocumentService using the injected IEmbeddingService
        // public async Task<float[]> GenerateEmbedding(string text)
        // {
        //     var embeddingClient = new OllamaEmbeddingClient(new Uri("http://localhost:11434"), "nomic-embed-text");
        //     var embeddingResponse = await embeddingClient.GenerateEmbeddingAsync(text);
        //     return embeddingResponse.Content.ToArray();
        // }
    }

    public class ChatRequestModel
    {
        public string Message { get; set; }
    }
}