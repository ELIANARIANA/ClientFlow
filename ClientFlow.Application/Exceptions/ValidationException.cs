namespace ClientFlow.Application.Exceptions
{
	public class ValidationException : Exception
	{
		public IDictionary<string, string[]> Errors { get; }

		public ValidationException(string message) : base(message) { }

		public ValidationException(IDictionary<string, string[]> errors)
		: base(FormatErrors(errors))
		{
			Errors = errors;
		}

		private static string FormatErrors(IDictionary<string, string[]> errors)
		{
			if (errors == null || !errors.Any()) return string.Empty;

			return string.Join("\n", errors.Select(e => $"- {e.Key}: {string.Join(", ", e.Value)}"));
		}
	}
}
