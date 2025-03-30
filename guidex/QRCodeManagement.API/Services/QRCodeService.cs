using Microsoft.EntityFrameworkCore;
using QRCodeManagement.API.Data;
using QRCodeManagement.API.DTOs;
using QRCodeManagement.API.Models;
using QRCodeManagement.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodeManagement.API.Services
{
    public class QRCodeService : IQRCodeService
    {
        private readonly ApplicationDbContext _context;

        public QRCodeService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Generic Pagination Result Class
        public class PaginatedResult<T>
        {
            public List<T> Items { get; set; } = new List<T>();
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public int TotalCount { get; set; }
            public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
            public bool HasPreviousPage => PageNumber > 1;
            public bool HasNextPage => PageNumber < TotalPages;

            public PaginatedResult(List<T> items, int totalCount, int pageNumber, int pageSize)
            {
                Items = items;
                TotalCount = totalCount;
                PageNumber = pageNumber;
                PageSize = pageSize;
            }
        }

        // Fetch QR codes by userId (Paginated)
        public async Task<PaginatedResult<QRCodeDetailsDTO>> GetQRCodesPagedAsync(int userId, int pageNumber = 1, int pageSize = 10)
        {
            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var query = _context.QRCodes
                .AsNoTracking()
                .Where(q => q.user_id == userId)
                .OrderByDescending(q => q.Id);

            var totalCount = await query.CountAsync();
            var qrCodes = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var itemIds = qrCodes.Select(q => q.item_id).ToList();
            var items = await _context.Items
                .Where(i => itemIds.Contains(i.Id))
                .ToDictionaryAsync(i => i.Id);

            var dtoList = qrCodes.Select(q => new QRCodeDetailsDTO
            {
                QRCode = q,
                Item = items.GetValueOrDefault(q.item_id),
                User = null
            }).ToList();

            return new PaginatedResult<QRCodeDetailsDTO>(dtoList, totalCount, pageNumber, pageSize);
        }

        // Fetch all QR codes for a user (No pagination)
        public async Task<List<QRCodeDetailsDTO>> GetAllQRCodesByUserIdAsync(int userId)
        {
            var qrCodes = await _context.QRCodes
                .AsNoTracking()
                .Where(q => q.user_id == userId)
                .OrderByDescending(q => q.Id)
                .ToListAsync();

            var itemIds = qrCodes.Select(q => q.item_id).ToList();
            var items = await _context.Items
                .Where(i => itemIds.Contains(i.Id))
                .ToDictionaryAsync(i => i.Id);

            return qrCodes.Select(q => new QRCodeDetailsDTO
            {
                QRCode = q,
                Item = items.GetValueOrDefault(q.item_id),
                User = null
            }).ToList();
        }
    }
}
