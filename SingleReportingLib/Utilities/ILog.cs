using System;
using System.Collections.Generic;
using System.Text;

 //Author: Asad Aziz
    //Date: 07/04/2008	
    public interface ILog
    {
        void info(object Message, object Source);
        void warn(object Message, object Source);
        void debug(object Message, object Source);
        void trace(object Message, object Source);
        void error(object Message, object Source);
        void fatal(object Message, object Source);

    }
    enum Level
    {
        INFO = 1,
        WARN = 2,
        DEBUG = 3,
        TRACE = 4,
        ERROR = 5,
        FATAL = 6
    }

