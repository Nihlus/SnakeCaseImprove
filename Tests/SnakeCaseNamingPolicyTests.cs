//
//  SnakeCaseNamingPolicyTests.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System.Text.Json;
using SnakeCaseImprove;
using Xunit;

namespace Tests;

/// <summary>
/// Tests the a snake-casing naming policy.
/// </summary>
public abstract class SnakeCaseNamingPolicyTests<TPolicy> where TPolicy : JsonNamingPolicy
{
    protected abstract TPolicy Policy { get; }

    /// <summary>
    /// Tests whether the naming policy converts names correctly for a variety of cases.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <param name="expected">The expected result.</param>
    [Theory]
    [InlineData("", "")]
    [InlineData("A", "a")]
    [InlineData("AB", "ab")]
    [InlineData("ABC", "abc")]
    [InlineData("ABCD", "abcd")]
    [InlineData("aAAaA", "a_a_aa_a")]
    [InlineData("IOStream", "io_stream")]
    [InlineData("IOStreamAPI", "io_stream_api")]
    [InlineData("already_snake", "already_snake")]
    [InlineData("SCREAMING_CASE", "screaming_case")]
    [InlineData("Ada_Case", "ada_case")]
    [InlineData("NormalPascalCase", "normal_pascal_case")]
    [InlineData("camelCase", "camel_case")]
    [InlineData("camelCaseAPI", "camel_case_api")]
    [InlineData("IOStreamAPIForReal", "io_stream_api_for_real")]
    [InlineData("OnceUponATime", "once_upon_a_time")]
    public void ConvertsCorrectly(string input, string expected)
    {
        Assert.Equal(expected, this.Policy.ConvertName(input));
    }
}

public class RemoraPolicyTests : SnakeCaseNamingPolicyTests<RemoraSnakeCaseNamingPolicy>
{
    protected override RemoraSnakeCaseNamingPolicy Policy => new();
}

public class JsonNetPolicyTests : SnakeCaseNamingPolicyTests<JsonNetSnakeCaseNamingPolicy>
{
    protected override JsonNetSnakeCaseNamingPolicy Policy => new();
}

public class JsonNetOptimizedSbPolicyTests : SnakeCaseNamingPolicyTests<JsonNetOptimizedSbSnakeCaseNamingPolicy>
{
    protected override JsonNetOptimizedSbSnakeCaseNamingPolicy Policy => new();
}

public class JsonNetStackallocPolicyTests : SnakeCaseNamingPolicyTests<JsonNetStackallocSnakeCaseNamingPolicy>
{
    protected override JsonNetStackallocSnakeCaseNamingPolicy Policy => new();
}

public class VelvetPolicyTests : SnakeCaseNamingPolicyTests<JaxSnakeCaseNamingPolicy>
{
    protected override JaxSnakeCaseNamingPolicy Policy => new();
}
