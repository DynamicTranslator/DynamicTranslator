namespace DynamicTranslator.Core
{
    using System;
    using Stateless;

    public enum State
    {
        Waiting,
        MouseDown,
        TextCaptured,
        DragStarted,
        DragFinished,
        MouseUp
    }

    public enum Trigger
    {
        Click,
        DoubleClick,
        ReleaseMouse,
        StartDrag,
        FinishDrag
    }

    public class TipsyMouse : IDisposable
    {
        readonly StateMachine<State, Trigger> stateMachine;

        public TipsyMouse(Action then)
        {
            this.stateMachine = new StateMachine<State, Trigger>(State.Waiting);

            this.stateMachine.Configure(State.Waiting)
                .Permit(Trigger.DoubleClick, State.TextCaptured)
                .Permit(Trigger.StartDrag, State.DragStarted)
                .PermitReentry(Trigger.ReleaseMouse)
                .PermitReentry(Trigger.ReleaseMouse)
                ;

            this.stateMachine.Configure(State.DragStarted)
                .Permit(Trigger.FinishDrag, State.TextCaptured)
                .Permit(Trigger.ReleaseMouse, State.Waiting)
                .PermitReentry(Trigger.StartDrag)
                ;

            this.stateMachine.Configure(State.TextCaptured)
                .Permit(Trigger.ReleaseMouse, State.Waiting)
                .Ignore(Trigger.DoubleClick)
                .Ignore(Trigger.StartDrag)
                .Ignore(Trigger.FinishDrag)
                .OnEntry(then)
                ;
        }


        public void Dispose() { }

        public void DoubleClick()
        {
            EnqueueTrigger(Trigger.DoubleClick);
        }

        public void StartDragging()
        {
            EnqueueTrigger(Trigger.StartDrag);
        }

        public void FinishDragging()
        {
            EnqueueTrigger(Trigger.FinishDrag);
        }

        public void Release()
        {
            EnqueueTrigger(Trigger.ReleaseMouse);
        }

        public State GetState()
        {
            lock (this.stateMachine)
                return this.stateMachine.State;
        }

        public bool IsInState(State state)
        {
            lock (this.stateMachine)
                return this.stateMachine.IsInState(state);
        }

        void EnqueueTrigger(Trigger @event)
        {
            lock (this.stateMachine)
                this.stateMachine.Fire(@event);
        }
    }
}