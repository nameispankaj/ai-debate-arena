# AI Debate Arena

A console app that pits two AI agents against each other (Defender vs. Offender) with a third Observer AI judging each round. The Observer awards points (+5 winner / -5 loser) and the app prints a final score table.

## Features
- Two debating agents with opposing stances.
- Observer AI judge for round winners.
- Fixed number of rounds with a 5‑second delay between outputs.
- Styled console output.
- Final 100‑point score table.

## Prerequisites
- .NET SDK (8.0 or newer recommended)
- Azure CLI logged in (`az login`)
- Azure AI Projects endpoint and project configured

## Setup
1. Clone the repository.
2. Update the endpoint in `Program.cs` if needed:
   ```csharp
   new Uri("https://<your-resource>.services.ai.azure.com/api/projects/<your-project>")
   ```
3. Restore and run:
   ```bash
   dotnet restore
   dotnet run
   ```

## Usage
- When prompted, enter a topic.
- The debate runs for the configured number of rounds.
- Scores are displayed at the end.

## Configuration
Edit these constants in `Program.cs`:
- `Rounds` (default: 3)
- `StartScore` (default: 50)
- `ScoreDelta` (default: 5)
- `WaitTime` (default: 5 seconds)

## Notes
- The Observer decides round winners based on clarity and relevance.
- You can swap the model name if needed.

## License
Add a license if you plan to share this publicly.
