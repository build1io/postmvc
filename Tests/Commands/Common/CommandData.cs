namespace Build1.PostMVC.Core.Tests.Commands.Common
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