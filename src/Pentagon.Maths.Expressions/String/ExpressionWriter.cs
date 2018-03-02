// -----------------------------------------------------------------------
//  <copyright file="ExpressionWriter.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Expressions.String
{
    class ExpressionWriter
    {
        public string Expression { get; private set; }

        public void AddChar(char ch)
        {
            Expression += ch;
        }
    }
}