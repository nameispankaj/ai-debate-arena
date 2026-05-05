using System;
using System.Collections;
using System.Threading.Tasks;
using Azure.AI.Projects;
using Azure.Identity;
using Microsoft.Agents.AI;

namespace AIDebateArena;

class Program
{
    private const int Rounds = 3;
    private const int StartScore = 50;
    private const int ScoreDelta = 5;
    private static readonly TimeSpan WaitTime = TimeSpan.FromSeconds(5);

    static async Task Main()
    {
        string? endpoint = Environment.GetEnvironmentVariable("AI_PROJECT_ENDPOINT");
        if (string.IsNullOrWhiteSpace(endpoint))
        {
            Console.WriteLine("Missing AI_PROJECT_ENDPOINT. Example:");
            Console.WriteLine("AI_PROJECT_ENDPOINT=\"https://<resource>.services.ai.azure.com/api/projects/<project>\"");
            return;
        }

        var client = new AIProjectClient(
            new Uri(endpoint),
            new AzureCliCredential());

        AIAgent defensiveAgent = client.AsAIAgent(
            model: "gpt-4.1-nano",
            instructions:
                "You are the Defender. Argue defensively, rebut offensive points, keep replies brief. " +
                "Use bullet points. Scoring rule: winner +5, loser -5.");

        AIAgent offensiveAgent = client.AsAIAgent(
            model: "gpt-4.1-nano",
            instructions:
                "You are the Offender. Argue offensively, challenge the topic, keep replies brief. " +
                "Use bullet points. Scoring rule: winner +5, loser -5.");

        AIAgent observerAgent = client.AsAIAgent(
            model: "gpt-4.1-nano",
            instructions:
                "You are the Observer Judge. Decide who won the round based on clarity and relevance. " +
                "Respond with ONLY one word: Defender or Offender.");

        AgentSession defensiveSession = await defensiveAgent.CreateSessionAsync();
        AgentSession offensiveSession = await offensiveAgent.CreateSessionAsync();
        AgentSession observerSession = await observerAgent.CreateSessionAsync();

        PrintBanner("AI Debate Arena");
        Console.Write("Topic: ");
        string? topic = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(topic)) return;

        int defenderScore = StartScore;
        int offenderScore = StartScore;

        Console.WriteLine();
        Console.WriteLine("Starting debate...");
        PrintDivider();

        for (int round = 1; round <= Rounds; round++)
        {
            PrintRoundHeader(round);

            var defenderResponse = await defensiveAgent.RunAsync(
                $"Topic: {topic}\nRound {round}: Provide defensive points (bullets).",
                defensiveSession);

            string defenderText = ExtractText(defenderResponse);
            PrintSection("DEFENDER", defenderText);
            await Task.Delay(WaitTime);

            var offenderResponse = await offensiveAgent.RunAsync(
                $"Topic: {topic}\nRound {round}: Provide offensive points (bullets) responding to defender.",
                offensiveSession);

            string offenderText = ExtractText(offenderResponse);
            PrintSection("OFFENDER", offenderText);
            await Task.Delay(WaitTime);

            var judgeResponse = await observerAgent.RunAsync(
                $"Topic: {topic}\nRound {round}:\nDefender:\n{defenderText}\nOffender:\n{offenderText}\nWho won? Reply only 'Defender' or 'Offender'.",
                observerSession);

            string winner = ExtractText(judgeResponse).Trim();
            ApplyScore(winner, ref defenderScore, ref offenderScore);

            PrintWinner(winner);
            await Task.Delay(WaitTime);
        }

        PrintDivider();
        PrintScoreTable(defenderScore, offenderScore);
        PrintDivider();

        string finalWinner = defenderScore == offenderScore ? "Draw" :
            defenderScore > offenderScore ? "Defender" : "Offender";
        Console.WriteLine($"Final Winner: {finalWinner}");
    }

    private static void ApplyScore(string winner, ref int defenderScore, ref int offenderScore)
    {
        if (winner.Equals("Defender", StringComparison.OrdinalIgnoreCase))
        {
            defenderScore += ScoreDelta;
            offenderScore -= ScoreDelta;
        }
        else if (winner.Equals("Offender", StringComparison.OrdinalIgnoreCase))
        {
            offenderScore += ScoreDelta;
            defenderScore -= ScoreDelta;
        }
    }

    private static string ExtractText(AgentResponse response)
    {
        if (response == null) return string.Empty;

        string[] propertyNames =
        {
            "Content", "OutputText", "Text", "Message", "Messages", "Value", "Response"
        };

        var type = response.GetType();

        foreach (var name in propertyNames)
        {
            var prop = type.GetProperty(name);
            if (prop == null) continue;

            var value = prop.GetValue(response);
            if (value == null) continue;

            if (value is string s)
                return s;

            if (value is IEnumerable enumerable)
            {
                string combined = "";
                foreach (var item in enumerable)
                {
                    if (item == null) continue;
                    combined += item + Environment.NewLine;
                }
                return combined.Trim();
            }

            return value.ToString() ?? string.Empty;
        }

        return response.ToString() ?? string.Empty;
    }

    // Styling helpers
    private static void PrintBanner(string title)
    {
        string line = new string('═', 60);
        Console.WriteLine($"╔{line}╗");
        Console.WriteLine($"║ {title.PadRight(58)} ║");
        Console.WriteLine($"╚{line}╝");
    }

    private static void PrintDivider()
    {
        Console.WriteLine(new string('─', 64));
    }

    private static void PrintRoundHeader(int round)
    {
        Console.WriteLine();
        Console.WriteLine($"======== ROUND {round} ========");
    }

    private static void PrintSection(string header, string body)
    {
        Console.WriteLine();
        Console.WriteLine($"[{header}]");
        Console.WriteLine(body);
    }

    private static void PrintWinner(string winner)
    {
        Console.WriteLine();
        Console.WriteLine($"Observer Winner: {winner}");
    }

    private static void PrintScoreTable(int defender, int offender)
    {
        Console.WriteLine("SCORE TABLE (OUT OF 100)");
        Console.WriteLine("+----------------+--------+");
        Console.WriteLine("| Agent          | Score  |");
        Console.WriteLine("+----------------+--------+");
        Console.WriteLine($"| Defender       | {defender,6} |");
        Console.WriteLine($"| Offender       | {offender,6} |");
        Console.WriteLine("+----------------+--------+");
    }
}
