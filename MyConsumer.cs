using Epam.FixAntenna.NetCore.FixEngine;
using Epam.FixAntenna.NetCore.Message;
using System.Threading.Channels;
using System.Threading.Tasks.Dataflow;

public class MyConsumer : IFixSessionListener
{
    private readonly IFixSession _session;
    private Timer monitorTimer;
    private object lockFixMessage;
    bool beginUsing;

    private readonly Channel<FixMessage> _fixChannel;
    private readonly CancellationTokenSource _ctSource;

    public MyConsumer(IFixSession session, Channel<FixMessage> fixChannel, CancellationTokenSource ctSource)
    {
        _fixChannel = fixChannel;
        _ctSource = ctSource;
        _session = session;
        lockFixMessage = new object();
        _ = StartConsuming();
        monitorTimer = new Timer( _ => DelayStart_OfFixMsgUsage(), null, 10000, Timeout.Infinite);
    }
    private void DelayStart_OfFixMsgUsage()
    {
        beginUsing = true;
        _ = monitorTimer.Change(-1, -1);			
    }


    /*  The FixAntenna library pushes fix messages to this method. See interface IFixSessionListener which inherits IFixMessageListener...
        This is then in effect the source of the producer
    */
    public void OnNewMessage(FixMessage fixMessage)
    {
        if(beginUsing)
        {				
            _ = OnNewMessageAsync(fixMessage);
        }
    }
    private async Task OnNewMessageAsync(FixMessage fixMessage)
    {
        await _fixChannel.Writer.WriteAsync(fixMessage, _ctSource.Token);		
    }
    public async Task StartConsuming()
    {
        await foreach (var fixBoodskap in _fixChannel.Reader.ReadAllAsync(cancellationToken: _ctSource.Token))
        {
            if(true)
            {
                if(true)
                {
                    // await Task.Run(() => CpuBoundTask());
                }
            }
        }			
    }	
    public void OnSessionStateChange(SessionState sessionState)
    {
        _ctSource.Cancel();
        // this callback is called upon session state change
        if (sessionState == SessionState.Disconnected)
        {
            // end this session
            _session.Dispose();
        }
    }
}