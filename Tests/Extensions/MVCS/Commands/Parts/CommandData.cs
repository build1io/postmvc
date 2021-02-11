namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Parts
{
    public sealed class CommandData
    {
        public int Count { get; private set; }

        public readonly int id;

        public CommandData(int id)
        {
            this.id = id;
        }

        public CommandData Increment()
        {
            Count++;
            return this;
        }
    }
}