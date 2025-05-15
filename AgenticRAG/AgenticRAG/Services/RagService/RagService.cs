using System.Text;
using AgenticRAG.Constants;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;


namespace AgenticRAG.Rag.Impl
{
	public class RagService
	{
        private Kernel kernel;
        private readonly AzureSearchService azureSearchService;

        public RagService(AzureSearchService azureSearchService)
		{
            
            var builder = Kernel.CreateBuilder();
            builder.AddAzureOpenAIChatCompletion(
                            "GPT4ov1",
                            Environment.GetEnvironmentVariable(EnvVariables.AzureOpenAIUrlEnvVar),
                            Environment.GetEnvironmentVariable(EnvVariables.AzureOpenAIKeyEnvVar)
                            );


            this.azureSearchService = azureSearchService;
            kernel = builder.Build();
            kernel.Plugins.AddFromObject(azureSearchService);
            kernel.ImportPluginFromType<WeatherPlugin>("searchWeather");

        }

        public async Task<string> GenerateResponseUsingTraditionalRag(string query)
        {

            string relevangtDocs = await azureSearchService.SearchHotels(query);

            if (string.IsNullOrEmpty(relevangtDocs))
            {
                relevangtDocs = "No context found for the user question";
            }

            string prompt = $@"
                    You are AI Trip Planner, which helps user for trip planning question based on the context provided.
                    You don't have access to external knowledge, so answer based on the current context. 
                    context : {relevangtDocs}
                    user: {query},
                ";

            var response = await kernel.InvokePromptAsync(prompt);
            return response.ToString();
        }

        public async Task<string> GenerateResponseUsingAgenticRag(string prompt)
        {
            StringBuilder result = new StringBuilder();
            PromptExecutionSettings settings = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };
            ChatHistory histroy = new ChatHistory();
            histroy.AddUserMessage(prompt);
            AgentThread thread = new ChatHistoryAgentThread(histroy);

            ChatCompletionAgent agent = new()
            {
                Name = "TripPlanner",
                Instructions = @"  
                Step 1: Plan the Task

                Carefully analyze the user query.

                - Break it down into clear, manageable sub-steps before taking any action.

                Step 2: Retrieve Relevant Context

                Search for and gather all relevant context available within the current conversation and system memory.

                - Ensure the retrieved context directly aligns with the user's request.

                Step 3: Reflect and Validate

                - Pause to critically reflect on the retrieved context and make sure it has temperature mentioned in faranite.

                Ask yourself: Does this context sufficiently answer the user's question?

                - If gaps are identified, determine whether further search or clarification is required.

                Ensure your reasoning aligns logically with the user intent and context.

                Step 4: Respond Using Context Only
                - optimize the search query
                - Construct your response solely using the information found in the provided context and user query.
                - Do not introduce any outside or assumed knowledge.

                Step 5: Share Stepwise Progress
                - Communicate progress transparently after completing each major step.
                - Clearly indicate what has been done and what is next.

                # Output Format
                    - Your response should begin with final answer and end with steps taken, tools invoked and reasoning.

                Note: You don't have access to external information. Answer should be solely on the knowledge in the current context.
                ",
                Kernel = kernel,
                Arguments = new(settings),
            };

            await foreach (ChatMessageContent response in agent.InvokeAsync(thread))
            {
                result.Append(response.Content);
            }

            return result.ToString();
        }

    }
}

