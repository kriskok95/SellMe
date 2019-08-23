﻿namespace SellMe.Common
{
    public static  class GlobalConstants
    {
        public const string InvalidAdIdErrorMessage = "Ad with the given id doesn't exist!";
        public const string InvalidPromotionIdErrorMessage = "Promotion with the given id doesn't exist!";
        public const string UserIsNotLogInErrorMessage = "User is not logged in!";
        public const string InvalidUserIdErrorMessage = "User id can't be null or empty!";

        public const string AdministratorRoleName = "Administrator";
        public const string ConditionBrandNewName = "Brand New";
        public const string ConditionUsedName = "Used";

        public const int PromotedAdsCountAtIndexPage = 16;
        public const int LatestAddedAdsCountAtIndexPage = 16;

        public const int AdDuration = 30;
    }
}
