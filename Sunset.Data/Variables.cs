using System.Collections.Generic;

namespace Sunset.Data
{
    /// <summary>
    /// 星期及節次條件組合
    /// </summary>
    public class Variables : IEnumerable<Variable>
    {
        //'local variable to hold collection
        private Dictionary<string, Variable> mVariables;

        public Variables()
        {
            mVariables = new Dictionary<string, Variable>();
        }

        public Variable Add(Variable NewVariable)
        {
            if (mVariables.ContainsKey(NewVariable.Name))
                return null;

            mVariables.Add(NewVariable.Name,NewVariable);

            return NewVariable;

            #region VB
            //Public Function Add(NewVariable As Variable) As Variable
            //    On Error GoTo DupVariable
            //    'set the properties passed into the method
            //    mCol.Add NewVariable, NewVariable.Name

            //    'return the object created
            //    Set Add = NewVariable
            //    Exit Function
            //DupVariable:
            //    Set Add = Nothing
            //End Function
            #endregion
        }

        public void Remove(string Key)
        {
            if (mVariables.ContainsKey(Key))
                mVariables.Remove(Key);

            #region VB
            //Public Sub Remove(vntIndexKey As Variant)
            //    mCol.Remove vntIndexKey
            //End Sub
            #endregion
        }

        public void Clear()
        {
            mVariables.Clear();
            mVariables = new Dictionary<string, Variable>();

            #region VB
            //Public Sub Clear()
            //    Set mCol = Nothing
            //    Set mCol = New Collection
            //End Sub
            #endregion
        }

        public bool Exists(string vName)
        {
            return mVariables.ContainsKey(vName);

            #region VB
            //Public Function Exists(VName As String) As Boolean
            //    Dim TestVar As Variable
            //    On Error GoTo NoSuchName

            //    Set TestVar = mCol(VName)
            //    Exists = True
            //    Exit Function
            //NoSuchName:
            //    Exists = False
            //End Function
            #endregion
        }

        public Variable this[string Key]
        {
            get 
            { 
                return mVariables.ContainsKey(Key)?mVariables[Key]:null;

                #region VB
                //Public Property Get Item(vntIndexKey As Variant) As Variable
                //    Set Item = mCol(vntIndexKey)
                //End Property
                #endregion
            }
        }

        public int Count { get { return mVariables.Count; } }

        #region IEnumerable<Variable> Members

        public IEnumerator<Variable> GetEnumerator()
        {
            return mVariables.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mVariables.Values.GetEnumerator();
        }

        #endregion
    }
}