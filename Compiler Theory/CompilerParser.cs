using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compiler_Theory
{
    /// <summary>
    /// ParseTreeNames
    /// </summary>
    public enum ParseTreeNames
    {
        Statements,Repeat_Statement, Assign_Statement, Read_Statement, Write_Statement, 
        If, Then, Else, Repeat, Until, Read, Write,
        Exp, Simple_Exp, Term, Factor,Identifier,Number,
        Comp_Op, Assign_Op, Add_Op, Mul_Op
    }
    public class CompilerParser
    {
        /// <summary>
        /// Symbol
        /// </summary>
        enum Symbol
        {
            If, Then, Else, End, Repeat, Until, Read, Write,
            EndFile, Error, Identifier, Number, Comment, ReservedWords, Char,
            Plus, Minus, Times, Equal, Division, GreaterThan, LessThan, LeftParentheses,
            RightParentheses, SemiColon, Assign, LessThanOrEqual, GreaterThanOrEqual, NotEqual
        }

        /// <summary>
        /// Data From Scanner
        /// </summary>
        private List<KeyValuePair<string, string>> ScannerData;
        private bool IsParserTreeDone;
        /// <summary>
        /// Create Parse Tree
        /// </summary>
        /// <param name="SD"></param>
        /// <param name="ParserTreeView"></param>
        public bool CreateParseTree(List<KeyValuePair<string, string>> SD, ref TreeNode ParserTreeRoot)
        {
            //ParserTreeView.Nodes.Clear();
            ScannerData = new List<KeyValuePair<string, string>>();
            foreach (var Data in SD)
            {
                if (Data.Value == Symbol.Error.ToString())
                {
                    MessageBox.Show("Error in Scanner");
                    return false;
                }
                if (Data.Value != Symbol.Comment.ToString())
                {
                    ScannerData.Add(new KeyValuePair<string, string>(Data.Key, Data.Value));
                }
            }

            ParserTreeRoot = Program();
            
            return IsParserTreeDone;
        }

        ///
        /// Option	[ ... ]
        /// Repetition	{ ... }
        ///
        
        /// <summary>
        /// Program ---> StatementSequence
        /// </summary>
        /// <returns></returns>
        private TreeNode Program()
        {
            IsParserTreeDone = true;
            TreeNode Node = null;
            int Index = 0;
            if (ScannerData.Count>1 && (!StatementSequence(ref Index, ref Node) || Index != ScannerData.Count - 1))
            {
                MessageBox.Show("Error in Parser");
                IsParserTreeDone = false;
            }
            return Node;
        }

        /// <summary>
        /// StatementSequence ---> Statement { ; Statement }
        /// </summary>
        /// <param name="CurrentIndex"></param>
        /// <param name="CurrentNode"></param>
        /// <returns></returns>
        private bool StatementSequence(ref int CurrentIndex, ref TreeNode CurrentNode)
        {
            TreeNode StatementSequenceNode = null;
            int OldIndex = CurrentIndex;
            if (Statement(ref CurrentIndex, ref StatementSequenceNode))
            {
                CurrentNode = CreateNode(ParseTreeNames.Statements, StatementSequenceNode);// Create Node
                OldIndex = CurrentIndex;
            }
            else
                return false;
            while (true)
            {
                if (ScannerData[CurrentIndex++].Value == Symbol.SemiColon.ToString())
                {
                    if (Statement(ref CurrentIndex, ref StatementSequenceNode))
                    {
                        CurrentNode = AddChildNode(CurrentNode, StatementSequenceNode);// Add child
                        OldIndex=CurrentIndex;
                    }
                    else
                        return false;
                }
                else
                {
                    CurrentIndex = OldIndex;
                    return true;
                }
            }

        }

        /// <summary>
        /// Statement ---> IfStatement | RepeatStatement | AssignStatement | ReadStatement | WriteStatement
        /// </summary>
        /// <param name="CurrentIndex"></param>
        /// <param name="CurrentNode"></param>
        /// <returns></returns>
        private bool Statement(ref int CurrentIndex, ref TreeNode CurrentNode)
        {
            int OldIndex = CurrentIndex;
            if (IfStatement(ref CurrentIndex, ref CurrentNode) || RepeatStatement(ref CurrentIndex, ref CurrentNode) || AssignStatement(ref CurrentIndex, ref CurrentNode) ||
            ReadStatement(ref CurrentIndex, ref CurrentNode) || WriteStatement(ref CurrentIndex, ref CurrentNode))
            {
                return true;
            }
            CurrentIndex = OldIndex;
            return false;
        }

        /// <summary>
        /// IfStatement ---> If Expression Then StatementSequence [ Else StatementSequence ] End
        /// </summary>
        /// <param name="CurrentIndex"></param>
        /// <param name="CurrentNode"></param>
        /// <returns></returns>
        private bool IfStatement(ref int CurrentIndex, ref TreeNode CurrentNode)
        {
            TreeNode exp = null;
            TreeNode stmts = null;
            TreeNode elseStmts = null;

            int OldIndex = CurrentIndex;
            if ( ScannerData[CurrentIndex++].Key == Symbol.If.ToString() && Expression(ref CurrentIndex, ref exp) &&
                ScannerData[CurrentIndex++].Key == Symbol.Then.ToString() && StatementSequence(ref CurrentIndex, ref stmts) &&
                ScannerData[CurrentIndex++].Key == Symbol.End.ToString() )
            {
                //Create Node
                CurrentNode = CreateNode(ParseTreeNames.If, exp);
                CurrentNode = AddChildNode(CurrentNode, CreateNode(ParseTreeNames.Then, stmts));

                return true;
            }
            CurrentIndex = OldIndex;
            if ( ScannerData[CurrentIndex++].Key == Symbol.If.ToString() && Expression(ref CurrentIndex, ref exp) &&
                ScannerData[CurrentIndex++].Key == Symbol.Then.ToString() && StatementSequence(ref CurrentIndex, ref stmts) &&
                ScannerData[CurrentIndex++].Key == Symbol.Else.ToString() &&  StatementSequence(ref CurrentIndex, ref elseStmts) &&
                ScannerData[CurrentIndex++].Key == Symbol.End.ToString() )
            {
                // Create Node
                CurrentNode = CreateNode(ParseTreeNames.If, exp);
                CurrentNode = AddChildNode(CurrentNode, CreateNode(ParseTreeNames.Then, stmts));
                CurrentNode = AddChildNode(CurrentNode, CreateNode(ParseTreeNames.Else, elseStmts));

                return true;
            }
            CurrentIndex = OldIndex;
            return false;
        }

        /// <summary>
        /// RepeatStatement ---> Repeat StatementSequence Until Expression
        /// </summary>
        /// <param name="CurrentIndex"></param>
        /// <param name="CurrentNode"></param>
        /// <returns></returns>
        private bool RepeatStatement(ref int CurrentIndex, ref TreeNode CurrentNode)
        {
            TreeNode stmtsNode = null;
            TreeNode exp = null;

            int OldIndex = CurrentIndex;
            if (ScannerData[CurrentIndex++].Key == Symbol.Repeat.ToString() && StatementSequence(ref CurrentIndex, ref stmtsNode) &&
                ScannerData[CurrentIndex++].Key == Symbol.Until.ToString() && Expression(ref CurrentIndex, ref exp) )
            {
                // Create Nodes
                CurrentNode = CreateNode(ParseTreeNames.Repeat_Statement, stmtsNode);
                CurrentNode = AddChildNode(CurrentNode, CreateNode(ParseTreeNames.Until,exp));
                return true;
            }
            CurrentIndex = OldIndex;
            return false;
        }
        
        /// <summary>
        /// AssignStatement ---> Identifier := Expression
        /// </summary>
        /// <param name="CurrentIndex"></param>
        /// <param name="CurrentNode"></param>
        /// <returns></returns>
        private bool AssignStatement(ref int CurrentIndex, ref TreeNode CurrentNode)
        {
            TreeNode idNode = null;
            TreeNode assignOpNode = null;
            TreeNode exp = null;

            int OldIndex = CurrentIndex;
            if ( ScannerData[CurrentIndex++].Value == Symbol.Identifier.ToString() &&
                ScannerData[CurrentIndex++].Value == Symbol.Assign.ToString() && Expression(ref CurrentIndex, ref exp) )
            {
                //Create Node
                idNode = CreateNode(ParseTreeNames.Identifier, ScannerData[OldIndex].Key);
                assignOpNode = CreateNode(ParseTreeNames.Assign_Op, ":=");
                CurrentNode = CreateNode(ParseTreeNames.Assign_Statement, idNode);
                CurrentNode = AddChildNode(CurrentNode, assignOpNode);
                CurrentNode = AddChildNode(CurrentNode, exp);

                return true;
            }
            CurrentIndex = OldIndex;
            return false;
        }
        
        /// <summary>
        /// ReadStatement ---> Read Identifier
        /// </summary>
        /// <param name="CurrentIndex"></param>
        /// <param name="CurrentNode"></param>
        /// <returns></returns>
        private bool ReadStatement(ref int CurrentIndex, ref TreeNode CurrentNode)
        {
            TreeNode idNode = null;

            int OldIndex = CurrentIndex;
            if (ScannerData[CurrentIndex++].Key == Symbol.Read.ToString() &&
                ScannerData[CurrentIndex++].Value == Symbol.Identifier.ToString())
            {
                // Create Node
                idNode = CreateNode(ParseTreeNames.Identifier, ScannerData[CurrentIndex - 1].Key);
                CurrentNode = CreateNode(ParseTreeNames.Read_Statement, ParseTreeNames.Read);
                CurrentNode = AddChildNode(CurrentNode, idNode);

                return true;
            }
            CurrentIndex = OldIndex;
            return false;
        }
        
        /// <summary>
        /// WriteStatement ---> Write Expression
        /// </summary>
        /// <param name="CurrentIndex"></param>
        /// <param name="CurrentNode"></param>
        /// <returns></returns>
        private bool WriteStatement(ref int CurrentIndex, ref TreeNode CurrentNode)
        {
            TreeNode expNode = null;

            int OldIndex = CurrentIndex;
            if ( ScannerData[CurrentIndex++].Key == Symbol.Write.ToString() &&
                Expression(ref CurrentIndex, ref expNode) )
            {
                // Create Node
                CurrentNode = CreateNode(ParseTreeNames.Write_Statement, ParseTreeNames.Write);
                CurrentNode = AddChildNode(CurrentNode, expNode);

                return true;
            }
            CurrentIndex = OldIndex;
            return false;
        }
        
        /// <summary>
        /// Expression ---> SimpleExpression [ ComparisonOperation SimpleExpression ]
        /// </summary>
        /// <param name="CurrentIndex"></param>
        /// <param name="CurrentNode"></param>
        /// <returns></returns>
        private bool Expression(ref int CurrentIndex, ref TreeNode CurrentNode)
        {
            TreeNode simpleExpNode = null;
            TreeNode compNode = null;

            int OldIndex = CurrentIndex;
            if ( SimpleExpression(ref CurrentIndex , ref simpleExpNode) )
            {
                // Create Node
                CurrentNode = CreateNode(ParseTreeNames.Exp, simpleExpNode);

                OldIndex = CurrentIndex;
            }
            else
            {
                CurrentIndex = OldIndex;
                return false;
            }

            if (ComparisonOperation(ref CurrentIndex , ref compNode))
            {
                if (SimpleExpression(ref CurrentIndex , ref simpleExpNode))
                {
                    // Add childs
                    CurrentNode = AddChildNode(CurrentNode, compNode);
                    CurrentNode = AddChildNode(CurrentNode, simpleExpNode);

                    OldIndex = CurrentIndex;
                }
                else
                {
                    CurrentIndex = OldIndex;
                    return false;
                }
            }
            CurrentIndex = OldIndex;
            return true;

        }

        /// <summary>
        /// ComparisonOperation ---> < | =
        /// </summary>
        /// <param name="CurrentIndex"></param>
        /// <param name="CurrentNode"></param>
        /// <returns></returns>
        private bool ComparisonOperation(ref int CurrentIndex, ref TreeNode CurrentNode)
        {
            int OldIndex = CurrentIndex;
            if (ScannerData[CurrentIndex].Value == Symbol.LessThan.ToString() ||
                ScannerData[CurrentIndex].Value == Symbol.GreaterThan.ToString() ||
                ScannerData[CurrentIndex].Value == Symbol.LessThanOrEqual.ToString() ||
                ScannerData[CurrentIndex].Value == Symbol.GreaterThanOrEqual.ToString() ||
                ScannerData[CurrentIndex].Value == Symbol.Equal.ToString() ||
                ScannerData[CurrentIndex].Value == Symbol.NotEqual.ToString() )
            {
                // Create Node
                CurrentNode = CreateNode(ParseTreeNames.Comp_Op, ScannerData[CurrentIndex].Key);

                CurrentIndex++;
                return true;
            }
            CurrentIndex = OldIndex;
            return false;
        }
        
        /// <summary>
        /// SimpleExpression ---> Term  { AddOperation Term }
        /// </summary>
        /// <param name="CurrentIndex"></param>
        /// <param name="CurrentNode"></param>
        /// <returns></returns>
        private bool SimpleExpression(ref int CurrentIndex, ref TreeNode CurrentNode)
        {
            TreeNode termNode = null;
            TreeNode addNode = null;

            int OldIndex = CurrentIndex;
            if (Term(ref CurrentIndex, ref termNode))
            {
                // Create Node
                CurrentNode = CreateNode(ParseTreeNames.Simple_Exp, termNode);

                OldIndex = CurrentIndex;
            }
            else
            {
                CurrentIndex = OldIndex;
                return false;
            }

            while (true)
            {
                if (AddOperation(ref CurrentIndex , ref addNode))
                {
                    if (Term(ref CurrentIndex, ref termNode))
                    {
                        // Add Childs
                        CurrentNode = AddChildNode(CurrentNode, addNode);
                        CurrentNode = AddChildNode(CurrentNode, termNode);

                        OldIndex = CurrentIndex;
                    }
                    else
                    {

                        CurrentIndex = OldIndex;
                        return false;
                    }
                }
                else
                {
                    CurrentIndex = OldIndex;
                    return true;
                }
            }
        }

        /// <summary>
        /// AddOperation ---> + | -
        /// </summary>
        /// <param name="CurrentIndex"></param>
        /// <param name="CurrentNode"></param>
        /// <returns></returns>
        private bool AddOperation(ref int CurrentIndex, ref TreeNode CurrentNode)
        {
            int OldIndex = CurrentIndex;
            if (ScannerData[CurrentIndex].Value == Symbol.Plus.ToString() ||
                ScannerData[CurrentIndex].Value == Symbol.Minus.ToString())
            {
                // Create Node
                CurrentNode = CreateNode(ParseTreeNames.Add_Op,ScannerData[CurrentIndex].Key);

                CurrentIndex++;
                return true;
            }
            CurrentIndex = OldIndex;
            return false;
        }

        /// <summary>
        /// Term ---> Factor  { MultiplicationOperation Factor }
        /// </summary>
        /// <param name="CurrentIndex"></param>
        /// <param name="CurrentNode"></param>
        /// <returns></returns>
        private bool Term(ref int CurrentIndex, ref TreeNode CurrentNode)
        {
            TreeNode factorNode = null;
            TreeNode MulNode = null;

            int OldIndex = CurrentIndex;
            if ( Factor(ref CurrentIndex, ref factorNode) )
            {
                // Create Node
                CurrentNode = CreateNode(ParseTreeNames.Term, factorNode);

                OldIndex = CurrentIndex;
            }
            else
            {
                CurrentIndex = OldIndex;
                return false;
            }
            while (true)
            {
                if (MultiplicationOperation(ref CurrentIndex, ref MulNode))
                {
                    if (Factor(ref CurrentIndex , ref factorNode))
                    {
                        // Add Nodes to Term Node
                        CurrentNode = AddChildNode(CurrentNode, MulNode);
                        CurrentNode = AddChildNode(CurrentNode, factorNode);

                        OldIndex = CurrentIndex;
                    }
                    else
                    {

                        CurrentIndex = OldIndex;
                        return false;
                    }
                }
                else
                {
                    CurrentIndex = OldIndex;
                    return true;
                }
            }
        }

        /// <summary>
        /// MultiplicationOperation ---> *
        /// </summary>
        /// <param name="CurrentIndex"></param>
        /// <param name="CurrentNode"></param>
        /// <returns></returns>
        private bool MultiplicationOperation(ref int CurrentIndex, ref TreeNode CurrentNode)
        {
            int OldIndex = CurrentIndex;
            if ( ScannerData[CurrentIndex].Value == Symbol.Times.ToString())
            {
                // Create Node
                CurrentNode = CreateNode(ParseTreeNames.Mul_Op, ScannerData[CurrentIndex].Key);
                
                CurrentIndex++;
                return true;
            }
            CurrentIndex = OldIndex;
            return false;
        }

        /// <summary>
        /// Factor ---> (Expression) | Number | Identifier
        /// </summary>
        /// <param name="CurrentIndex"></param>
        /// <param name="CurrentNode"></param>
        /// <returns></returns>
        private bool Factor(ref int CurrentIndex, ref TreeNode CurrentNode)
        {
            TreeNode expNode = null;
            TreeNode numOrId = null;

            int OldIndex = CurrentIndex;
            if (ScannerData[CurrentIndex++].Value == Symbol.LeftParentheses.ToString() && Expression(ref CurrentIndex, ref expNode) &&
                ScannerData[CurrentIndex++].Value == Symbol.RightParentheses.ToString() )
            { 
                // Create Node
                CurrentNode = CreateNode(ParseTreeNames.Factor, expNode);

                return true;
            }
            CurrentIndex = OldIndex;
            if ( ScannerData[CurrentIndex].Value == Symbol.Number.ToString() ||
                ScannerData[CurrentIndex].Value == Symbol.Identifier.ToString() )
            {
                // Create Node
                numOrId = CreateNode(ScannerData[CurrentIndex].Value, ScannerData[CurrentIndex].Key);
                CurrentNode = CreateNode(ParseTreeNames.Factor, numOrId);

                CurrentIndex++;
                return true;
            }
            CurrentIndex = OldIndex;
            return false;
        }

        /// <summary>
        /// CreateNode
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private TreeNode CreateNode(string nodeName,string value)
        {
            TreeNode CurrentNode = new TreeNode(nodeName);
            CurrentNode.Name = nodeName;
            CurrentNode.Nodes.Add(value);
            CurrentNode.Nodes[0].Name = value;

            return CurrentNode;
        }

        /// <summary>
        /// CreateNode
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private TreeNode CreateNode(string nodeName, TreeNode value)
        {
            TreeNode CurrentNode = new TreeNode(nodeName);
            CurrentNode.Name = nodeName;
            CurrentNode.Nodes.Add(value);

            return CurrentNode;
        }
        private TreeNode CreateNode(ParseTreeNames nodeName)
        {
            TreeNode node = new TreeNode(nodeName.ToString());
            node.Name = nodeName.ToString();
            return node;
        }
        private TreeNode CreateNode(ParseTreeNames nodeName, TreeNode value)
        {
            TreeNode CurrentNode = new TreeNode(nodeName.ToString());
            CurrentNode.Name = nodeName.ToString();
            CurrentNode.Nodes.Add(value);

            return CurrentNode;
        }
        private TreeNode CreateNode(ParseTreeNames nodeName, ParseTreeNames value)
        {
            TreeNode CurrentNode = new TreeNode(nodeName.ToString());
            CurrentNode.Name = nodeName.ToString();
            CurrentNode.Nodes.Add(value.ToString());
            CurrentNode.Nodes[0].Name = value.ToString();

            return CurrentNode;
        }
        private TreeNode CreateNode(ParseTreeNames nodeName, string value)
        {
            TreeNode CurrentNode = new TreeNode(nodeName.ToString());
            CurrentNode.Name = nodeName.ToString();
            CurrentNode.Nodes.Add(value);
            CurrentNode.Nodes[0].Name = value;

            return CurrentNode;
        }
        /// <summary>
        /// AddChildNode
        /// </summary>
        /// <param name="Parent"></param>
        /// <param name="Child"></param>
        /// <returns></returns>
        private TreeNode AddChildNode(TreeNode Parent,TreeNode Child)
        {
            Parent.Nodes.Add(Child);
            return Parent;
        }
    }
}
