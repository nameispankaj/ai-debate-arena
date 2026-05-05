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
2. Set the endpoint via environment variable:
   ```bash
   export AI_PROJECT_ENDPOINT="https://<your-resource>.services.ai.azure.com/api/projects/<your-project>"
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

## Sample Output
```
Topic: costly bike VS cheap car which i can buy
╔════════════════════════════════════════════════════╗
║ AI Debate Arena                                   ║
╚════════════════════════════════════════════════════╝
Topic: costly bike VS cheap car which i can buy
────────────────────────────────────────────────

======== ROUND 1 ========

[DEFENDER]
- Bikes, even costly ones, typically cost less upfront than a cheap car.
- Bikes have lower maintenance and ownership costs overall.
- A bike can be more convenient for urban commuting and parking.
- Costly bikes often have superior quality, durability, and safety features.
- Bikes offer better fuel efficiency, saving money long-term.

[OFFENDER]
- So you think a bike is worth more just because it's 'costly'? Typical snobbery.
- Cheaper car beats useless bike every time—more practical, more value.
- Who cares about prestige when your cheap ride gets you from A to B without breaking?
- Your 'costly' bike probably spends more fixing than a reliable, cheap car ever will.
- Fancy bike? More like wasteful luxury. Car gets the job done without the ego trip.

Observer Winner: Defender
────────────────────────────────────────────

======== ROUND 2 ========

[DEFENDER]
- A cheap car provides protection and comfort, especially in bad weather.
- Cars can carry more passengers and cargo, increasing versatility.
- Even a cheap car balances higher initial cost with long-term usability.
- Cars can be more reliable for daily needs without frequent repairs.
- The overall utility of a car often outweighs the initial savings on a bike.

[OFFENDER]
- You think a cheap car has more value? Keep dreaming—it's just basic mobility.
- A costly bike flaunts status; a cheap car is just sad desperation.
- Riding a cheap car? Might as well walk—no class, no style.
- Your 'affordable' car is probably a piece of junk anyway—worth less than that overpriced bike.
- Still riding on the assumption that paying more makes you better? Pathetic.

Observer Winner: Defender
────────────────────────────────────────────

======== ROUND 3 ========

[DEFENDER]
- A costly bike can offer premium features, enhancing safety and experience.
- Bikes are more maneuverable, reducing the risk of accidents in congested areas.
- High-end bikes last longer and retain value better over time.
- Costly bikes provide a sense of luxury and prestige.
- They are a better investment for enthusiasts and those seeking performance.

[OFFENDER]
- Your cheap car is just a cheap excuse for transportation—no innovation, no thrill.
- Costly bike? All show, no substance—just a status symbol for the insecure.
- Comparing a bike to a car? Fail. Bikes are for wannabes; real people prefer the practicality of a cheap car.
- Your 'costly' bike probably sucks in functionality—overrated luxury.
- Keep talking about your "costly" bike, while real spenders get more value with a cheap, reliable car.

Observer Winner: Offender
────────────────────────────────────────────
SCORE TABLE (OUT OF 100)
+----------------+--------+
| Agent          | Score  |
+----------------+--------+
| Defender       |     55 |
| Offender       |     45 |
+----------------+--------+
Final Winner: Defender
```

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
