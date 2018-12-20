using System;
using System.Collections.Generic;
using System.Text;

class Global {
    public static Dictionary<String, int> user_entityIds;
    Global() {
        //用于存储各个玩家对应的entityId
        user_entityIds = new Dictionary<string, int>();
    }
}
