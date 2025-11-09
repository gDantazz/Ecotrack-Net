using EcoTrack.Application.Dtos;
using EcoTrack.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcoTrack.Net.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductsController(ProductService svc, LinkGenerator links) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDto>> GetById(Guid id, CancellationToken ct)
        => (await svc.GetAsync(id, ct)) is { } dto ? Ok(WithItemLinks(dto)) : NotFound();

    [HttpGet]
    public async Task<ActionResult> Search([FromQuery] string? q, [FromQuery] string? category, [FromQuery] string? sortBy = "name", [FromQuery] string? sortDir = "asc", [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 10;

        var (items, total) = await svc.SearchAsync(q, category, sortBy, sortDir, page, pageSize, ct);
        var result = new
        {
            Items = items.Select(WithItemLinks),
            Total = total,
            Page = page,
            PageSize = pageSize,
            Links = CreatePagingLinks(q, category, sortBy, sortDir, page, pageSize, total)
        };
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateProductRequest req, CancellationToken ct)
    {
        var id = await svc.CreateAsync(req, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest req, CancellationToken ct)
    { await svc.UpdateAsync(id, req, ct); return NoContent(); }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    { await svc.DeleteAsync(id, ct); return NoContent(); }

    private object WithItemLinks(ProductDto p)
    {
        var self = links.GetUriByAction(HttpContext, action: nameof(GetById), controller: "Products", values: new { id = p.Id });
        return new
        {
            p.Id,
            p.Name,
            p.Category,
            p.CaloriesPer100g,
            p.Co2PerUnit,
            p.Barcode,
            Links = new[] {
                new { rel = "self", href = self, method = "GET" },
                new { rel = "update", href = links.GetUriByAction(HttpContext, action: nameof(Update), controller: "Products", values: new { id = p.Id }), method = "PUT" },
                new { rel = "delete", href = links.GetUriByAction(HttpContext, action: nameof(Delete), controller: "Products", values: new { id = p.Id }), method = "DELETE" }
            }
        };
    }

    private object[] CreatePagingLinks(string? q, string? category, string? sortBy, string? sortDir, int page, int pageSize, int total)
    {
        var lastPage = (int)Math.Ceiling(total / (double)pageSize);
        string? Make(int p) => links.GetUriByAction(HttpContext, action: nameof(Search), controller: "Products", values: new { q, category, sortBy, sortDir, page = p, pageSize });
        var list = new List<object> { new { rel = "self", href = Make(page), method = "GET" } };
        if (page > 1) list.Add(new { rel = "prev", href = Make(page - 1), method = "GET" });
        if (page < lastPage) list.Add(new { rel = "next", href = Make(page + 1), method = "GET" });
        list.Add(new { rel = "first", href = Make(1), method = "GET" });
        list.Add(new { rel = "last", href = Make(lastPage), method = "GET" });
        return list.ToArray();
    }
}
