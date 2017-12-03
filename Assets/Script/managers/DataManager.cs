using System;
using System.Collections.Generic;
public class DataManager
{
    #region fixedArea
    private static DataManager m_dataMgr = null;
    private DataManager() { }
    public static DataManager instance
    {
        get
        {
            if (m_dataMgr == null)
            {
                m_dataMgr = new DataManager();
            }
            return m_dataMgr;
        }
    }
    #endregion 

}

