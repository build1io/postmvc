using System;

namespace Build1.PostMVC.Core.MVCS.Mediation.Api
{
    public interface IMediationBinding
    {
        Type ViewType      { get; }
        Type ViewInterface { get; }
        Type MediatorType  { get; }
    }
}