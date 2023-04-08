using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*============================================================
**
** Class:  KnowledgeSource
**
** Purpose: Class that receives updates from the Blackboard
** where this is contained along with other KnowledgeSources.
**
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/
namespace LittleGuyGames
{
[Serializable]
public abstract class KnowledgeSource
{
    /// <summary>Updates KnowledgeSource.</summary>
    /// <param name="blackboard">Blackboard that is invoking this callback.</param>
    public abstract void Update(Blackboard blackboard);
}
}