namespace Frame
{
    public class ManagerCenter
    {
        #region fixed area
        private static ManagerCenter m_managerCenter;
        private GameFacade m_facade = null;
        private ResourceManager m_resMgr = null;
        public static ManagerCenter instance
        {
            get
            {
                if (m_managerCenter == null)
                {
                    m_managerCenter = new ManagerCenter();
                }
                return m_managerCenter;
            }
        }
        public void Init(GameFacade facade)
        {
            m_facade = facade;
        }
        public ResourceManager resMgr
        {
            get
            {
                if (m_resMgr == null)
                {
                    m_resMgr = m_facade.transform.GetComponent<ResourceManager>();
                }
                return m_resMgr;
            }
        }
        public DataManager dataMgr
        {
            get
            {
                return DataManager.instance;
            }
        }
        #endregion

    }
}
