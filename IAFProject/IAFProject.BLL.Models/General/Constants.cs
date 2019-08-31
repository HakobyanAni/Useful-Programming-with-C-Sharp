namespace IAFProject.BLL.Models.General
{
    public static class Constants
    {
        public static string SuccesMessage = "Success";
        public static string messageSubjectForUserSignUp = "Test Mail";
        public static string messageBodyForUserSignUp = $@"http://***************/api/user/emailconfirm?userName=";
        public static string messageSubjectForUserDeleteJob = "User account deleting notification.";
        public static string messageBodyForUserDeleteJob = "Your account is deleted, so as you didn't you it one year.";
    }
}