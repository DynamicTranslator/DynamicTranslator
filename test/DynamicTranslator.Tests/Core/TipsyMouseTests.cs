namespace DynamicTranslator.Tests.Core
{
    using DynamicTranslator.Core;
    using FluentAssertions;
    using Xunit;

    public class TipsyMouseTests
    {
        [Fact]
        public void When_doubleClick_happens_then_it_should_be_in_TextCaptured_state()
        {
            var triggered = false;
            using var mouse = new TipsyMouse(() => { triggered = true; });

            mouse.DoubleClick();

            mouse.IsInState(State.TextCaptured).Should().BeTrue();
            triggered.Should().BeTrue();
        }

        [Fact]
        public void When_drag_and_finish_then_it_should_be_in_TextCaptured_state()
        {
            var triggered = false;
            using var mouse = new TipsyMouse(() => triggered = true);

            mouse.StartDragging();
            mouse.FinishDragging();

            mouse.IsInState(State.TextCaptured).Should().Be(true);
            triggered.Should().BeTrue();
        }

        [Fact]
        public void When_multiple_double_drag_happens_then_it_should_continue_with_draggingFinish()
        {
            var triggered = false;
            using var mouse = new TipsyMouse(() => triggered = true);

            mouse.StartDragging();
            mouse.StartDragging();
            mouse.FinishDragging();

            mouse.IsInState(State.TextCaptured).Should().Be(true);
            triggered.Should().BeTrue();
        }

        [Fact]
        public void When_TextCaptured_with_Dragging_and_Release_comes_then_it_should_return_initial_state()
        {
            var triggered = false;
            var mouse = new TipsyMouse(() => triggered = true);

            mouse.StartDragging();
            mouse.FinishDragging();

            mouse.Release();

            mouse.IsInState(State.Waiting).Should().BeTrue();
            triggered.Should().Be(true);
        }

        [Fact]
        public void When_TextCaptured_with_DoubleClick_and_Release_comes_then_it_should_return_initial_state()
        {
            var triggered = false;
            using var mouse = new TipsyMouse(() => triggered = true);

            mouse.DoubleClick();
            mouse.Release();

            mouse.IsInState(State.Waiting).Should().BeTrue();
            triggered.Should().BeTrue();
        }
    }
}