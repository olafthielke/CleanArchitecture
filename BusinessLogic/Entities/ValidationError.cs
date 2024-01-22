namespace BusinessLogic.Entities
{
    public sealed record Error(string Code, string? Message = null);


    public static class ValidationErrors
    {
        public static readonly Error MissingCustomerRegistration = new("Validation.MissingCustomerRegistration",
            "Missing customer registration data.");

        public static readonly Error MissingFirstName = new("Validation.MissingFirstName",
            "Missing first name.");

        public static readonly Error MissingLastName = new("Validation.MissingLastName",
            "Missing last name.");

        public static readonly Error MissingEmailAddress = new("Validation.MissingEmailAddress",
            "Missing email address.");

        public static readonly Error DuplicateCustomerEmailAddress = new("Validation.DuplicateCustomerEmailAddress",
            "This email address already exists in the system.");
    }
}
