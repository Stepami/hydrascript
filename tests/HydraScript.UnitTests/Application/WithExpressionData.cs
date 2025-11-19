using HydraScript.Domain.BackEnd;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Create;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Write;
using HydraScript.Domain.BackEnd.Impl.Values;
using HydraScript.Domain.FrontEnd.Parser;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.ComplexLiterals;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.UnitTests.Application;

public sealed class WithExpressionData : TheoryData<IAbstractSyntaxTree, AddressedInstructions>
{
    public WithExpressionData()
    {
        // let obj = {x: 1;} with {}
        Add(
            new AbstractSyntaxTree(
                new AssignmentExpression(
                    new MemberExpression(new IdentifierReference("obj")),
                    new WithExpression(
                        new ObjectLiteral([
                            new Property(
                                new IdentifierReference("x"),
                                new Literal(new TypeIdentValue(new IdentifierReference("number")), 1,
                                    "(1, 15)-(1, 16)"))
                        ]),
                        new ObjectLiteral([])))),
            [
                new CreateObject("obj"),
                new DotAssignment("obj", new Constant("x"), new Constant(1))
            ]);

        // let copyFrom = {x: 0;}
        // let obj = copyFrom with {x: 1;}
        Add(
            new AbstractSyntaxTree(
                new AssignmentExpression(
                    new MemberExpression(new IdentifierReference("obj")),
                    new WithExpression(
                        new IdentifierReference("copyFrom"),
                        new ObjectLiteral([
                            new Property(
                                new IdentifierReference("x"),
                                new Literal(new TypeIdentValue(new IdentifierReference("number")), 1,
                                    "(1, 15)-(1, 16)"))
                        ]))
                    {
                        ComputedCopiedProperties = []
                    })),
            [
                new CreateObject("obj"),
                new DotAssignment("obj", new Constant("x"), new Constant(1))
            ]);
    }
}