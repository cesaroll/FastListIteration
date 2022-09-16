using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace FastListIteration;

[MemoryDiagnoser(false)]
public class Benchmarks
{
  private static readonly Random Rng = new Random(80085);

  [Params(100, 100_000, 1_000_000)]
  public int Size {get; set; }

  private List<int> _items;

  [GlobalSetup]
  public void Setup() {
    _items = Enumerable.Range(1, Size).Select(x => Rng.Next()).ToList();
  }

  [Benchmark]
  public void For() {
    for (var i = 0; i < _items.Count; i++) {
      var item = _items[i];
    }
  }

  [Benchmark]
  public void Foreach() {
    foreach (var item in _items) {
    }
  }

  [Benchmark]
  public void Foreach_Linq() {
    _items.ForEach(item => {});
  }

  [Benchmark]
  public void Parallel_ForEach() {
    Parallel.ForEach(_items, i => {});
  }

  [Benchmark]
  public void Parallel_Linq() {
    _items.AsParallel().ForAll(i => {});
  }

  [Benchmark]
  public void ForEach_Span() {
    foreach (int item in CollectionsMarshal.AsSpan(_items))
    {
    }
  }

  [Benchmark]
  public void For_Span() {
    var asSpan = CollectionsMarshal.AsSpan(_items);
    for (var i = 0; i < asSpan.Length; i++) {
      var item = _items[i];
    }
  }
}
