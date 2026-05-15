using OldenEraTemplateEditor.Models;
using OldenEraTemplateEditor.Views;

namespace OldenEraTemplateEditor.Services
{
    public class RmgService
    {
        public static void ApplyGlobalDto(Rmg rmg, GlobalDto dto)
        {
            rmg.rmgTemplate.Name = dto.name;
            rmg.rmgTemplate.GameMode = dto.gameMode;

            // WinCondition: convert label to id
            if (dto.winCondition != null)
            {
                int idx = Array.IndexOf(KnownValues.VictoryConditionLabels, dto.winCondition);
                rmg.rmgTemplate.DisplayWinCondition = idx >= 0 ? KnownValues.VictoryConditionIds[idx] : "win_condition_1";
            }
            else
            {
                rmg.rmgTemplate.DisplayWinCondition = "win_condition_1";
            }

            rmg.rmgTemplate.SizeX = dto.sizeX;
            rmg.rmgTemplate.SizeZ = dto.sizeZ;

            rmg.rmgTemplate.GameRules ??= new GameRules();
            rmg.rmgTemplate.GameRules.HeroCountMin = dto.heroCountMin;
            rmg.rmgTemplate.GameRules.HeroCountMax = dto.heroCountMax;
            rmg.rmgTemplate.GameRules.HeroCountIncrement = dto.heroCountIncrement;

            rmg.rmgTemplate.GameRules.HeroHireBan = false;
            rmg.rmgTemplate.GameRules.FactionLawsExpModifier = 1.0;
            rmg.rmgTemplate.GameRules.AstrologyExpModifier = 1.0;
            rmg.rmgTemplate.GameRules.Bonuses = new();

            switch (rmg.rmgTemplate.DisplayWinCondition)
            {
                case "win_condition_1":
                    rmg.rmgTemplate.GameRules.WinConditions = GetStandardWinConditionsWinConditions();
                    break;
                case "win_condition_3":
                    rmg.rmgTemplate.GameRules.WinConditions = GetLostStartingCityWinConditions();
                    break;
                case "win_condition_5":
                    rmg.rmgTemplate.GameRules.WinConditions = GetHoldCityWinConditions();
                    break;
                case "win_condition_6":
                    rmg.rmgTemplate.GameRules.WinConditions = GetTournamentWinConditions();
                    break;
            }
            if (rmg.rmgTemplate.GameMode == "SingleHero")
            {
                rmg.rmgTemplate.GameRules.WinConditions.LostStartHero = true;
            }
        }

        private static WinConditions GetStandardWinConditionsWinConditions()
        {
            WinConditions WinConditions = new();
            WinConditions.Classic = true;
            WinConditions.Desertion = true;
            WinConditions.DesertionDay = 3;
            WinConditions.DesertionValue = 3000;
            WinConditions.HeroLighting = true;
            WinConditions.HeroLightingDay = 1;
            WinConditions.LostStartCity = false;
            WinConditions.LostStartHero = false;
            return WinConditions;
        }
        private static WinConditions GetLostStartingCityWinConditions()
        {
            WinConditions WinConditions = new();
            WinConditions.Classic = true;
            WinConditions.Desertion = true;
            WinConditions.DesertionDay = 3;
            WinConditions.DesertionValue = 3000;
            WinConditions.HeroLighting = true;
            WinConditions.HeroLightingDay = 1;
            WinConditions.LostStartCity = true;
            WinConditions.LostStartCityDay = 3;
            WinConditions.LostStartHero = false;
            return WinConditions;
        }
        private static WinConditions GetHoldCityWinConditions()
        {
            WinConditions WinConditions = new();
            WinConditions.Classic = true;
            WinConditions.Desertion = true;
            WinConditions.DesertionDay = 3;
            WinConditions.DesertionValue = 3000;
            WinConditions.HeroLighting = true;
            WinConditions.HeroLightingDay = 1;
            WinConditions.LostStartCity = false;
            WinConditions.LostStartCityDay = 3;
            WinConditions.LostStartHero = false;
            WinConditions.CityHold = true;
            WinConditions.CityHoldDays = 3;
            return WinConditions;
        }
        private static WinConditions GetTournamentWinConditions()
        {
            WinConditions WinConditions = new();
            WinConditions.Classic = true;
            WinConditions.Desertion = true;
            WinConditions.DesertionDay = 3;
            WinConditions.DesertionValue = 3000;
            WinConditions.HeroLighting = true;
            WinConditions.HeroLightingDay = 1;
            WinConditions.LostStartCity = false;
            WinConditions.LostStartHero = true;
            WinConditions.GladiatorArena = false;
            WinConditions.GladiatorArenaRegistrationStartWork = false;
            WinConditions.GladiatorArenaRegistrationStartFight = true;
            WinConditions.GladiatorArenaDaysDelayStart = 21;
            WinConditions.GladiatorArenaCountDay = 8;
            // TODO
            WinConditions.Tournament = true;
            WinConditions.TournamentPointsToWin = 2;
            WinConditions.TournamentSaveArmy = true;
            WinConditions.TournamentDays = [3, 3, 3];
            WinConditions.TournamentAnnounceDays = [7, 14, 21];
            WinConditions.ChampionSelectRule = "StartHero";

            return WinConditions;

        }
    }
}
