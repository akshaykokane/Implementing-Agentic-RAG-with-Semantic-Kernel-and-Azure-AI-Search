# 🧠 Agentic RAG Service using .NET 8, Azure OpenAI & Semantic Kernel

This repository demonstrates how to build a **Retrieval-Augmented Generation (RAG)** service using **Microsoft Semantic Kernel SDK**, **.NET 8**, and **Azure AI Search**. It includes both **Traditional RAG** and **Agentic RAG** implementations with structured prompt execution and dynamic agent reasoning.

---

## 📌 Features

- 🔎 Traditional RAG with Azure Cognitive Search and contextual prompt injection
- 🤖 Agentic RAG using Semantic Kernel's `ChatCompletionAgent` and `AgentThread`
- 🌦 Weather plugin integration (example)
- 💡 Modular architecture with plugin support and step-by-step reasoning

---

---

## 🚀 Getting Started

### 🔧 Configuration

Before running the project, you need to replace placeholder values with your actual Azure credential in [launchSettings.json](https://github.com/akshaykokane/Implementing-Agentic-RAG-with-Semantic-Kernel-and-Azure-AI-Search/blob/main/AgenticRAG/AgenticRAG/Properties/launchSettings.json)

```
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "AZURE_OPENAI_URL": "https://<RESOURCENAME>.openai.azure.com",
        "AZURE_OPENAI_KEY": "<ADD_KEY>",
        "AZURE_AI_SEARCH_URL": "https://<RESOURCENAME>.search.windows.net",
        "AZURE_AI_SEARCH_KEY": "<ADD_KEY>",
        "EMBEDDING_DEPLOYMENT_NAME": "<REPLACE_WITH_DEPLOYMENT_NAME>",
        "GPT_DEPLOYMENT_NAME": "<LLM_MODEL_DEPLOYMENT_NAME>",
        "INDEX_NAME": "<AI_SEARCH_INDEX_NAME>"
      },
```

Also, ensure your Azure AI Search setup and embedding model configurations are correctly referenced in your `AzureSearchService.cs`.

---

### ▶️ How to Run

#### 📌 Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
* Valid Azure OpenAI and Azure AI Search access
* Optional: Visual Studio or VS Code for debugging and running

#### 🛠 Build and Run the Project

You can run the service locally using the .NET CLI:

```bash
git clone https://github.com/your-username/AgenticRAG.git
cd AgenticRAG
dotnet restore
dotnet build
dotnet run
```

This will start the web API and allow you to access endpoints that trigger traditional and agentic RAG behavior.

---

### 📬 API Endpoints (Example)

> Note: Update based on actual controller methods in your project

```
POST /rag/traditional
{
  "query": "Find best hotels in Sedona"
}

POST /rag/agentic
{
  "prompt": "Plan a weekend trip for family to Acadia National Park"
}
```



## 🚀 Usage

### 🔹 Traditional RAG

```csharp
var result = await ragService.GenerateResponseUsingTraditionalRag("Best hotels in Bar Harbor?");
Console.WriteLine(result);
```

This version:

* Embeds the user query
* Retrieves relevant documents using Azure AI Search
* Constructs a prompt using retrieved context
* Sends it to the LLM for response generation

---

### 🔹 Agentic RAG

```csharp
var result = await ragService.GenerateResponseUsingAgenticRag("What's the weather like for a trip to Acadia National Park?");
Console.WriteLine(result);
```

This version:

* Breaks down the prompt into sub-tasks
* Optimizes and refines search queries
* Invokes tools (e.g., weather)
* Reflects and reasons on the context before finalizing the answer

The agent returns:

* Final answer
* Reasoning trace
* Tools and context used

---

## 🧩 Plugins

Example Weather Plugin added as:

```csharp
kernel.ImportPluginFromType<WeatherPlugin>("searchWeather");
```

You can build and register your own plugins to expand capabilities.

---

## 📚 Technologies Used

* [.NET 8](https://dotnet.microsoft.com/)
* [Microsoft Semantic Kernel](https://github.com/microsoft/semantic-kernel)
* [Azure OpenAI](https://learn.microsoft.com/en-us/azure/cognitive-services/openai/)
* [Azure Cognitive Search](https://learn.microsoft.com/en-us/azure/search/)

---
