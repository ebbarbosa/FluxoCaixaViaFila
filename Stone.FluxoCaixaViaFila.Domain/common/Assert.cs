namespace Stone.FluxoCaixaViaFila.Domain.Assertives
{
    public class Assert
    {

        public static bool IsTrue(bool condition, string exceptionMessage)
        {
            if (!condition)
                throw new ValidationException(exceptionMessage);
            return true;
        }
    }
}