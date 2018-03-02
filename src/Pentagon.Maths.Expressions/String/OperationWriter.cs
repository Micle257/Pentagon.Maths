// -----------------------------------------------------------------------
//  <copyright file="OperationWriter.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Expressions.String
{
    using System.Collections.Generic;

    class OperationWriter
    {
        public OperationWriter()
        {
            Exprs = new List<ExpressionWriter>();
        }

        public OperationWriter(ExpressionWriter ex1)
        {
            Exprs = new List<ExpressionWriter> {ex1};
        }

        public OperationWriter(ExpressionWriter ex1, ExpressionWriter ex2)
        {
            Exprs = new List<ExpressionWriter> {ex1, ex2};
        }

        public List<ExpressionWriter> Exprs { get; set; }
        public char Oper { get; set; }
    }
}