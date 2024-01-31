using System.Threading.Channels;
using Epam.FixAntenna.NetCore.Common;
using Epam.FixAntenna.NetCore.FixEngine;
using Epam.FixAntenna.NetCore.Message;


var dataSession = new SessionParameters
{
    FixVersion = FixVersion.Fix42,
    HeartbeatInterval = 60,
    Host = "IP",
    Port = 1,
    SenderCompId = "ID",
    TargetCompId = "ID",
    ForceSeqNumReset = 0
};

IFixSession session = dataSession.CreateNewFixSession();
Channel<FixMessage> fixChannel = Channel.CreateUnbounded<FixMessage>();
CancellationTokenSource ctSource = new CancellationTokenSource();

MyConsumer myConsumer = new MyConsumer(session, fixChannel, ctSource);
