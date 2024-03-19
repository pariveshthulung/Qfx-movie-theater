using QFX.data;
using QFX.Generic.Interface;

namespace QFX;

public class DeleteShow : IDeleteShow
{
    private readonly ApplicationDbContext _context;
    public DeleteShow(ApplicationDbContext context)
    {
        _context = context;
    }
    void IDeleteShow.DeleteShow(List<long> ID)
    {
        var Show = _context.Shows.Where(x=>ID.Contains(x.ID)).ToList();
        _context.Shows.RemoveRange(Show);
        _context.SaveChanges();
    }
}
