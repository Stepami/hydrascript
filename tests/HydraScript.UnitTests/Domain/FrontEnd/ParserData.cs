using System.Collections;

namespace HydraScript.UnitTests.Domain.FrontEnd;

public class ParserSuccessTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return ["-21"];
        yield return ["!false"];
        yield return ["~[]"];
        yield return ["x = ([1,2] ++ [3,4])::0"];
        yield return ["i[0].j"];
        yield return ["i[0].j()"];
        yield return ["i = 1"];
        yield return ["i[0] = 1"];
        yield return ["i[a.b][1].x(1)"];
        yield return ["(1 + 2) * (3 - (2 / 2)) as string"];
        yield return ["return {x:1;y:2;}"];
        yield return ["while (~arr != 0) { arr::0 continue }"];
        yield return ["if (!(true || (false && false))) { break } else { break }"];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}