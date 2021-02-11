namespace Build1.PostMVC.Extensions.MVCS.Mediation
{
    public interface IView
    {
        IMediator Mediator { get; }

        void SetMediator(IMediator mediator);
    }
}