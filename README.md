
# LLM Integration Project with Ollama and .NET (ASP.NET Core)

This project showcases a fundamental integration of Large Language Models (LLMs) into an ASP.NET Core web application. It demonstrates how your .NET application can communicate with a locally running Ollama server to perform AI tasks such as text generation and embedding creation.

## Project Overview

This project provides a simple web application with a chat interface. It leverages a local Large Language Model (Llama 3) for conversational AI and an embedding model (nomic-embed-text) for tasks like generating text representations for Retrieval Augmented Generation (RAG) patterns (demonstrated with dummy data for context). The application is designed to communicate with a pre-configured Ollama server running on the user's local machine.

### Key Features:

* **Local LLM Inference:** Communicates with a locally running Ollama instance, allowing for private and cost-effective LLM operations without external API calls.
* **ASP.NET Core Integration:** Seamlessly integrates LLM capabilities into a modern .NET web application.
* **Microsoft.Extensions.AI:** Utilizes Microsoft's official abstraction layer for AI, providing a clean and extensible way to interact with models.
* **Embedding Generation:** Demonstrates how to generate text embeddings using Ollama's embedding models (e.g., `nomic-embed-text`).
* **Text Generation:** Shows how to use a chat completion model (e.g., `Llama 3`) for conversational AI.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine.

### Prerequisites

Before you begin, ensure you have the following installed:

* **[Git](https://git-scm.com/downloads)**: For cloning the repository.
* **.NET SDK 8.0 or later**: For building and running the ASP.NET Core application.
* **[Ollama](https://ollama.com/download)**: The Ollama server must be installed and running on your local machine.

### Installation and Setup

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/hkhajei/LLM_Integration_with_AspNetCoreProject.git](https://github.com/hkhajei/LLM_Integration_with_AspNetCoreProject.git)
    ```

2.  **Install and Run Ollama:**
    If you haven't already, download and install Ollama from [ollama.com/download](https://ollama.com/download).
    After installation, ensure the Ollama server is running. You can usually verify this by opening a terminal and running `ollama list` (it should not give a "connection refused" error).

3.  **Pull Required Models in Ollama:**
    This ASP.NET Core application is configured to use `llama3` for chat and `nomic-embed-text` for embeddings. You need to download these models into your Ollama instance. Open a terminal and run:

    ```bash
    ollama pull llama3
    ollama pull nomic-embed-text
    ```
    * **Note:** `llama3` is a large model (around 4.7GB) and `nomic-embed-text` is around 274MB. The download time will depend on your internet connection. Ensure both models are fully downloaded by running `ollama list` and verifying their presence.

4.  **Run the ASP.NET Core Application:**
    Navigate to the `AspNetCoreProject` directory within the cloned repository:

    Then, run the application:

    ```bash
    dotnet run
    ```
    * The application will start, usually on `https://localhost:7210` (check the console output for the exact URL).
    * **Important:** If you encounter `System.Net.Http.HttpRequestException: Response status code does not indicate success: 404 (Not Found)` errors during startup, it means your Ollama server is not running, or the required models (`llama3`, `nomic-embed-text`) are not fully loaded/available in Ollama. Please re-check steps 2 and 3.

5.  **Access the Application:**
    Open your web browser and navigate to the URL displayed in the console after `dotnet run` (e.g., `https://localhost:7210`).

## Project Structure

* `AspNetCoreProject/`: Contains the entire ASP.NET Core web application solution.
    * `Program.cs`: Main entry point, where LLM services are initialized using `Microsoft.Extensions.AI`.
    * `Controllers/ChatController.cs`: Handles chat and Q&A interactions with the LLM.
    * `Models/Services/DocumentService.cs`: Manages document embeddings and retrieval logic.

## Technologies Used

* **.NET 8 (ASP.NET Core)**: The framework for the web application.
* **Ollama**: For running and serving LLMs locally.
* **Microsoft.Extensions.AI**: Microsoft's official abstraction library for integrating AI models in .NET.
* **Llama 3**: A powerful open-source LLM used for chat and generation.
* **Nomic Embed Text**: An efficient open-source embedding model used for RAG.

## Contribution

Feel free to fork this repository, open issues, and submit pull requests.

## License

This project is open-source and available under the [MIT License](LICENSE).
