using System;

namespace Stone.FluxoCaixaViaFila.Domain
{
	public class ValidationException : ApplicationException
	{
		public ValidationException(string message) : base(message)
		{
		}
	}
}