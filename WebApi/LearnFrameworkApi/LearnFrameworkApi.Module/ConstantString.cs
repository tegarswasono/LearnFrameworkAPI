using System.ComponentModel;

namespace LearnFrameworkApi.Module
{
    public static class ConstantString
    {
        //System
        public const string DataNotFound1 = "Data Not Found";
        public const string DataNotFound = "{0} Not Found";
        public const string DataAlreadyExist = "{0} Already Exist";
        public const string ProcessSuccessfully = "Process Successfully";
        public const string DeleteSuccessfully = "Delete Successfully";
        public const string ThisDataIsUsedInOtherTransaction = "This data is used in other transactions";

        //CustomAttribute Validation Message
        public const string ShouldBeBiggerThan0 = "should be bigger than 0";
        public const string ShouldBeBiggerThanOrEqualsTo0 = "should be bigger than or equals to 0";
        public const string ShouldBeLowerThanOrEqualsTo0 = "should be lower than or equals to 0";
        public const string ShouldBeNotContainHtmlTag = "should be not contain html tag";

        //Path
        public const string PathProfilePicture = "wwwroot/Upload/ProfilePicture/";
    }
}
