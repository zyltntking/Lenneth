using System;
using System.IO;

namespace Lenneth.Core.FrameWork.BouncyCastle.crypto.tls
{
    public class TlsFatalAlert
        : IOException
    {
        private readonly byte alertDescription;

        public TlsFatalAlert(byte alertDescription)
            : this(alertDescription, null)
        {
        }

        public TlsFatalAlert(byte alertDescription, Exception alertCause)
            : base(tls.AlertDescription.GetText(alertDescription), alertCause)
        {
            this.alertDescription = alertDescription;
        }

        public virtual byte AlertDescription
        {
            get { return alertDescription; }
        }
    }
}
