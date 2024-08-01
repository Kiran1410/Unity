using System;
public class JenkinsBuildConstants
{
    public const string ARGS_AB_ENABLED = "AB_Enabled";
    public const string ARGS_BUILD_TYPE_DEBUG = "debug";
    public const string ARGS_BUILD_SCENE = ".unity";
    public const string ARGS_BUILD_NUM = "buildNum";
    public const string ARGS_JOB_NAME = "jobName";
    public const string ARGS_LABEL_NAME = "LABEL";
    public const string ARGS_BRAND_ID_NAME = "brandID";
    public const string ARGS_BUCKET_NAME = "bucketName";

    public const string LABEL_US = "US";
    public const string LABEL_COM = "COM";
    public const string LABEL_ROW = "ROW";
    public const string LABEL_SBZA = "SBZA";
    public const string LABEL_BWIN_ES = "BwinES";
    public const string LABEL_BWIN_BE = "BwinBE";
    public const string LABEL_BWIN_COM = "BwinCom";
    public const string LABEL_SB_COM = "SBCom";
    public const string LABEL_ONTARIO = "Ontario";
    public const string LABEL_BET_MGM = "BetMGM";
    public const string LABEL_GAMEBOOKERS = "GameBooker";
    public const string LABEL_PARTYPOKER_COM = "PartyPoker";
    public const string LABEL_PREMIUM = "Premium";
    public const string LABEL_SB_UK = "SBUK";


    public const string EDITOR_LOBBY_SCENE = "EditorLobby";
    public const string SPLASH_SCENE = "0SplashScreen";

    public const String BUNDLE_TYPE = "BUNDLE_TYPE";
    public const string BUNDLE_TYPE_ASSET_BUNDLES = "AssetBundles";
    public const string BUNDLE_TYPE_ADDRESSABLES = "Addressables";

    public enum USGames
    {
        ivyamericanroulette,
        ivybisonfury,
        ivyblackjackpro,
        ivypremiumblackjack,
        ivymgmgrandmillions,
        ivyborgatacashspinner

    }

    public enum ROWGames
    {
        ivyslidereuroroulette,
        ivysliderblackjack,
        ivyminislidereuroroulette,
        ivysliderperfectblackjackpro

    }

    public enum SBZAGames
    {
        ivypremiumblackjack,
        ivyeuroroulettepro
    }

    public enum BwinESGames
    {
        ivypremiumblackjack,
        ivyeuroroulettepro
    }

    public enum BwinBEGames
    {
        ivypremiumblackjack
    }

    public enum BwinComGames
    {
        ivyslidereuroroulette,
        ivysliderblackjack,
        ivysliderperfectblackjackpro
    }

    public enum GameBookersGames
    {
        ivyslidereuroroulette,
        ivysliderblackjack,
        ivysliderperfectblackjackpro
    }

    public enum PartyPokerComGames
    {
        ivyslidereuroroulette,
        ivysliderblackjack,
        ivysliderperfectblackjackpro
    }

    public enum SBUKGames
    {
        ivyslidereuroroulette,
        ivysliderblackjack,
        ivysliderperfectblackjackpro
    }

    public enum SBComGames
    {
        ivyslidereuroroulette,
        ivysliderblackjack,
        ivysliderperfectblackjackpro
    }

    public enum PremiumGames
    {
        ivyslidereuroroulette,
        ivysliderblackjack,
        ivysliderperfectblackjackpro
    }


    public enum OntarioGames
    {
        ivypremiumblackjack,
        ivybisonfury,
        ivyamericanroulette,
        ivyeuroroulettepro
    }

    public enum BetMgmGames
    {
        ivypremiumblackjack,
        ivybisonfury,
        ivyamericanroulette
    }

}
