namespace Build1.PostMVC.Core.MVCS.Mediation
{
    public interface IView
    {
        IMediator Mediator { get; }

        void SetMediator(IMediator mediator);
    }
}