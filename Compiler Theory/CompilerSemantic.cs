using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compiler_Theory
{
    class CompilerSemantic
    {
        Dictionary<string, double> AllValues = new Dictionary<string, double>();

        /// <summary>
        /// Create Semantic Tree
        /// </summary>
        /// <param name="ParserTreeRoot"></param>
        public void CreateSemanticTree(ref TreeNode ParserTreeRoot)
        {
            Statements(ref ParserTreeRoot);
        }

        /// <summary>
        /// Statements
        /// </summary>
        /// <param name="node"></param>
        private void Statements(ref TreeNode node)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                TreeNode child = node.Nodes[i];
                switch ((ParseTreeNames)Enum.Parse(typeof(ParseTreeNames), child.Text))
                {
                    case ParseTreeNames.If:
                        If(ref child);
                        editNode(ref node, child, i);
                        break;
                    case ParseTreeNames.Assign_Statement:
                        Assign_Statement(ref child);
                        editNode(ref node, child, i);
                        break;
                    case ParseTreeNames.Read_Statement:
                        Read(ref child);
                        editNode(ref node, child, i);
                        break;
                    case ParseTreeNames.Write_Statement:
                        Write(ref child);
                        editNode(ref node, child, i);
                        break;
                    case ParseTreeNames.Repeat_Statement:
                        Repeat(ref child);
                        editNode(ref node, child, i);
                        break;
                }
            }
        }

        /// <summary>
        /// If
        /// </summary>
        /// <param name="node"></param>
        private void If(ref TreeNode node)
        {
            TreeNode expNode = nodeOfKey(node, ParseTreeNames.Exp);
            Exp(ref expNode);

            string[] tokens = split(expNode.Text);
            bool Succes = false;
            if (!hasAlpha(tokens[1]))
            {
                if (Convert.ToDouble(tokens[1]) != 0)
                {
                    expNode.Text = tokens[0] + format("True");
                    Succes = true;
                }
                else
                    expNode.Text = tokens[0] + format("False");
            }
            else
            {
                expNode.Text = tokens[0] + format("IDK");
            }

            editNode(ref node, expNode);

            if (Succes)
            {
                TreeNode Then = nodeOfKey(node, ParseTreeNames.Then);
                TreeNode statements = nodeOfKey(Then, ParseTreeNames.Statements);
                Statements(ref statements);
                editNode(ref Then, statements);
                editNode(ref node, Then);
            }
            else
            {
                //else missed
                TreeNode Else = nodeOfKey(node, ParseTreeNames.Else);
                if (Else == null)
                    return;
                TreeNode elseStatements = nodeOfKey(Else, ParseTreeNames.Statements);
                Statements(ref elseStatements);
                editNode(ref Else, elseStatements);
                editNode(ref node, Else);
            }

        }

        /// <summary>
        /// Repeat
        /// </summary>
        /// <param name="node"></param>
        private void Repeat(ref TreeNode node)
        {
            TreeNode statements = nodeOfKey(node, ParseTreeNames.Statements);
            Statements(ref statements);
            editNode(ref node, statements);

            TreeNode Until = nodeOfKey(node, ParseTreeNames.Until);
            TreeNode expNode = nodeOfKey(Until, ParseTreeNames.Exp);
            Exp(ref expNode);

            string[] tokens = split(expNode.Text);
            if (!hasAlpha(tokens[1]))
            {
                if (Convert.ToDouble(tokens[1]) != 0)
                {
                    expNode.Text = tokens[0] + format("True");
                }
                else
                    expNode.Text = tokens[0] + format("False");
            }
            else
            {
                expNode.Text = tokens[0] + format("IDK");
            }

            editNode(ref Until, expNode);
            editNode(ref node, Until);
        }

        /// <summary>
        /// Assign_Statement
        /// </summary>
        /// <param name="node"></param>
        private void Assign_Statement(ref TreeNode node)
        {
            TreeNode expNode = nodeOfKey(node, ParseTreeNames.Exp);
            Exp(ref expNode);
            string[] tokens = split(expNode.Text);
            expNode.Text = tokens[0];
            editNode(ref node, expNode);

            // tokens = split(expNode.FirstNode.Text);

            string ID = nodeOfKey(node, ParseTreeNames.Identifier).FirstNode.Text;
            // ID := Unknown value
            if (hasAlpha(tokens[1]))
            {
                if (this.AllValues.ContainsKey(ID))
                {
                    this.AllValues.Remove(ID);
                }
                return;
            }
            // ID := Known value
            if (this.AllValues.ContainsKey(ID))
                this.AllValues[ID] = Convert.ToDouble(tokens[1]);
            else
                this.AllValues.Add(ID, Convert.ToDouble(tokens[1]));
        }

        /// <summary>
        /// Read
        /// </summary>
        /// <param name="node"></param>
        private void Read(ref TreeNode node)
        {
            string ID = nodeOfKey(node, ParseTreeNames.Identifier).FirstNode.Text;
            if (this.AllValues.ContainsKey(ID))
            {
                this.AllValues.Remove(ID);
            }
        }

        /// <summary>
        /// Write
        /// </summary>
        /// <param name="node"></param>
        private void Write(ref TreeNode node)
        {
            TreeNode expNode = nodeOfKey(node, ParseTreeNames.Exp);
            Exp(ref expNode);
            editNode(ref node, expNode);
        }

        /// <summary>
        /// Exp
        /// </summary>
        /// <param name="node"></param>
        private void Exp(ref TreeNode node)
        {
            if (node.Nodes.Count == 1)
            {
                TreeNode simpleExpNode = nodeOfKey(node, ParseTreeNames.Simple_Exp);
                Simple_Exp(ref simpleExpNode);
                editNode(ref node, simpleExpNode);

                string value = split(simpleExpNode.Text)[1];

                node.Text += format(value);


            }
            else
            {
                int index = indexOfKey(node, ParseTreeNames.Simple_Exp);

                // Simple_Exp 1
                TreeNode simpleExpNode = node.Nodes[index];
                Simple_Exp(ref simpleExpNode);
                editNode(ref node, simpleExpNode, index);
                string firstTerm = split(simpleExpNode.Text)[1];

                index += 2;
                // Simple_Exp 2
                simpleExpNode = node.Nodes[index];
                Simple_Exp(ref simpleExpNode);
                editNode(ref node, simpleExpNode, index);
                string secondTerm = split(simpleExpNode.Text)[1];

                // Do Comparison
                if (!hasAlpha(firstTerm) && !hasAlpha(secondTerm))
                {
                    int val = (Compare(firstTerm, secondTerm, nodeOfKey(node, ParseTreeNames.Comp_Op).FirstNode.Text) == true) ? 1 : 0;
                    node.Text += format(val.ToString());
                }
                else if (!hasAlpha(firstTerm) && this.AllValues.ContainsKey(secondTerm))
                {
                    int val = (Compare(firstTerm, this.AllValues[secondTerm].ToString(), nodeOfKey(node, ParseTreeNames.Comp_Op).FirstNode.Text) == true) ? 1 : 0;
                    node.Text += format(val.ToString());
                }
                else if (this.AllValues.ContainsKey(firstTerm) && !hasAlpha(secondTerm))
                {
                    int val = (Compare(this.AllValues[firstTerm].ToString(), secondTerm, nodeOfKey(node, ParseTreeNames.Comp_Op).FirstNode.Text) == true) ? 1 : 0;
                    node.Text += format(val.ToString());
                }
                else
                {
                    node.Text += format("IDK");
                }
            }
        }

        /// <summary>
        /// Simple_Exp
        /// </summary>
        /// <param name="node"></param>
        private void Simple_Exp(ref TreeNode node)
        {
            double res = 0;
            string name = "";

            for (int i = 0; i < node.Nodes.Count; i += 2)
            {
                if (i == 0)
                {
                    TreeNode termNode = node.Nodes[i];
                    Term(ref termNode);

                    string[] tokens = split(termNode.Text);
                    termNode.Text = tokens[0];

                    if (!hasAlpha(tokens[1]))
                    {
                        res = Convert.ToDouble(tokens[1]);
                    }
                    else
                    {
                        name += tokens[1];
                    }
                }
                else
                {
                    TreeNode termNode = node.Nodes[i];
                    Term(ref termNode);

                    string[] tokens = split(termNode.Text);
                    termNode.Text = tokens[0];
                    if (!hasAlpha(tokens[1]))
                    {
                        if (node.Nodes[i - 1].FirstNode.Text == "+")
                        {
                            res += Convert.ToDouble(tokens[1]);
                        }
                        else
                        {
                            res -= Convert.ToDouble(tokens[1]);
                        }
                    }
                    else
                    {
                        if (node.Nodes[i - 1].FirstNode.Text == "+")
                        {
                            if (name != "")
                                name += " + " + tokens[1];
                            else
                                name += tokens[1];
                        }
                        else
                        {
                            name += " - " + tokens[1];
                        }
                    }
                    editNode(ref node, termNode, i);
                }
            }

            if (name != "")
            {
                if (res != 0)
                    name += "+ (" + res.ToString() + ")";
                node.Text += format(name);
            }
            else
            {
                node.Text += format(res.ToString());
            }
        }

        /// <summary>
        /// Term
        /// </summary>
        /// <param name="node"></param>
        private void Term(ref TreeNode node)
        {
            double res = 1;
            string name = "";

            for (int i = 0; i < node.Nodes.Count; i += 2)
            {
                if (i == 0)
                {
                    TreeNode factorNode = node.Nodes[i];
                    Factor(ref factorNode);

                    string[] tokens = split(factorNode.Text);
                    factorNode.Text = tokens[0];

                    if (!hasAlpha(tokens[1]))
                    {
                        res = Convert.ToDouble(tokens[1]);
                    }
                    else
                    {
                        name += tokens[1];
                    }
                }
                else
                {
                    TreeNode factorNode = node.Nodes[i];
                    Factor(ref factorNode);

                    string[] tokens = split(factorNode.Text);
                    factorNode.Text = tokens[0];
                    if (!hasAlpha(tokens[1]))
                    {
                        if (node.Nodes[i - 1].FirstNode.Text == "*")
                        {
                            res *= Convert.ToDouble(tokens[1]);
                        }
                    }
                    else
                    {
                        if (node.Nodes[i - 1].FirstNode.Text == "*")
                        {
                            if (name != "")
                                name += " * " + tokens[1];
                            else
                                name += tokens[1];
                        }
                    }
                    editNode(ref node, factorNode, i);
                }
            }

            if (name != "")
            {
                if (res != 1)
                    name += "* (" + res.ToString() + ")";
                node.Text += format(name);
            }
            else
            {
                node.Text += format(res.ToString());
            }
        }

        /// <summary>
        /// Factor
        /// </summary>
        /// <param name="node"></param>
        private void Factor(ref TreeNode node)
        {
            if (node.Nodes.ContainsKey(ParseTreeNames.Number.ToString()))
            {
                TreeNode x = nodeOfKey(node, ParseTreeNames.Number);
                node.Text += format(nodeOfKey(node, ParseTreeNames.Number).FirstNode.Text);
            }
            else if (node.Nodes.ContainsKey(ParseTreeNames.Identifier.ToString()))
            {
                TreeNode id = nodeOfKey(node, ParseTreeNames.Identifier);
                if (this.AllValues.ContainsKey(id.FirstNode.Text))
                {
                    node.Text += format(this.AllValues[id.FirstNode.Text].ToString());
                }
                else
                {
                    node.Text += format(id.FirstNode.Text);
                }
            }
            else if (node.Nodes.ContainsKey(ParseTreeNames.Exp.ToString()))
            {
                TreeNode expNode = nodeOfKey(node, ParseTreeNames.Exp);
                Exp(ref expNode);

                string[] tokens = split(expNode.Text);
                node.Text += format(tokens[1]);
                expNode.Text = tokens[0];

                editNode(ref node, expNode);
            }
        }

        /// <summary>
        /// Compare
        /// </summary>
        /// <param name="firstTerm"></param>
        /// <param name="secondTerm"></param>
        /// <param name="Comp_Op"></param>
        /// <returns></returns>
        private bool Compare(string firstTerm, string secondTerm, string Comp_Op)
        {
            switch (Comp_Op)
            {
                case "<":
                    return Convert.ToDouble(firstTerm) < Convert.ToDouble(secondTerm);
                case "<=":
                    return Convert.ToDouble(firstTerm) <= Convert.ToDouble(secondTerm);
                case ">":
                    return Convert.ToDouble(firstTerm) > Convert.ToDouble(secondTerm);
                case ">=":
                    return Convert.ToDouble(firstTerm) >= Convert.ToDouble(secondTerm);
                case "=":
                    return Convert.ToDouble(firstTerm) == Convert.ToDouble(secondTerm);
                case "!=":
                    return Convert.ToDouble(firstTerm) != Convert.ToDouble(secondTerm);
            }
            return false;
        }

        /// <summary>
        /// nodeOfKey
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private TreeNode nodeOfKey(TreeNode node, ParseTreeNames key)
        {
            int x = node.Nodes.IndexOfKey(key.ToString());
            if (x == -1)
                return null;
            return node.Nodes[node.Nodes.IndexOfKey(key.ToString())];
        }

        /// <summary>
        /// indexOfKey
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private int indexOfKey(TreeNode node, ParseTreeNames key)
        {
            return node.Nodes.IndexOfKey(key.ToString());
        }

        /// <summary>
        /// editNode
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        private void editNode(ref TreeNode parent, TreeNode child)
        {
            string[] token = split(child.Text);
            ParseTreeNames childName = (ParseTreeNames)Enum.Parse(typeof(ParseTreeNames), token[0]);

            parent.Nodes[indexOfKey(parent, childName)] = child;
        }

        /// <summary>
        /// editNode
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <param name="index"></param>
        private void editNode(ref TreeNode parent, TreeNode child, int index)
        {
            parent.Nodes[index] = child;
        }

        /// <summary>
        /// format
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string format(string str)
        {
            return "@" + str;
        }

        /// <summary>
        /// split
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string[] split(string str)
        {
            return str.Split('@');
        }

        /// <summary>
        /// hasAlpha
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool hasAlpha(string str)
        {
            foreach (char ch in str)
            {
                if (ch != '.' && Char.IsLetter(ch))
                    return true;
            }
            return false;
        }
    }
}