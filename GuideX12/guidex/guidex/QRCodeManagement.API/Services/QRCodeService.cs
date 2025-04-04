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
        
        public async Task<PaginatedResponse<QRCodeDetailsDTO>> GetQRCodesAsync(PaginationParameters paginationParams)
        {
            var query = _context.QRCodes
                .Select(q => new QRCodeDetailsDTO
                {
                    QRCode = q,
                    Item = _context.Items.FirstOrDefault(i => i.Id == q.ItemId),
                    User = _context.Users.FirstOrDefault(u => u.Id == q.Id)
                });

            var totalCount = await query.CountAsync();
            
            var items = await query
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();
            
            return new PaginatedResponse<QRCodeDetailsDTO>
            {
                Items = items,
                PageNumber = paginationParams.PageNumber,
                PageSize = paginationParams.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)paginationParams.PageSize)
            };
        }

        public async Task<int> CreateQRCodeAsync(QRCode qrCode, int userId)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == userId);
                
                if (user == null)
                    return 0;
                
                qrCode.Id = userId;
                
                _context.QRCodes.Add(qrCode);
                await _context.SaveChangesAsync();
                
                return qrCode.Id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<QRCodeDetailsDTO> GetQRCodeDetailsAsync(int qrCodeId, int userId)
        {
            var qrCode = await _context.QRCodes
                .FirstOrDefaultAsync(q => q.Id == qrCodeId);
            
            if (qrCode == null)
                return null;
            
            var item = await _context.Items
                .FirstOrDefaultAsync(i => i.Id == qrCode.ItemId);
            
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);
            
            return new QRCodeDetailsDTO
            {
                QRCode = qrCode,
                Item = item,
                User = user
            };
        }
        
        public async Task<bool> UpdateItemInfoAsync(int qrCodeId, int userId, Item updatedItem)
        {
            var qrCode = await _context.QRCodes
                .FirstOrDefaultAsync(q => q.Id == qrCodeId && q.Id == userId);
            
            if (qrCode == null)
                return false;
            
            var item = await _context.Items
                .FirstOrDefaultAsync(i => i.Id == qrCode.ItemId);
            
            if (item == null)
                return false;
            
            item.name_EN = updatedItem.name_EN;
            item.description_EN = updatedItem.description_EN;
            item.color_EN = updatedItem.color_EN;
            item.name_AR = updatedItem.name_AR;
            item.description_AR = updatedItem.description_AR;
            item.color_AR = updatedItem.color_AR;
            
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> DeleteItemInfoAsync(int qrCodeId, int userId)
        {
            var qrCode = await _context.QRCodes
                .FirstOrDefaultAsync(q => q.Id == qrCodeId && q.Id == userId);
            
            if (qrCode == null)
                return false;
            
            var item = await _context.Items
                .FirstOrDefaultAsync(i => i.Id == qrCode.ItemId);
            
            if (item == null)
                return false;
            
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}