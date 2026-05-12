using server.Models;
using server.Models.Enums;
using server.Repositories.Interfaces;
using server.Services.Interfaces;
using server.DTOs;
using AutoMapper;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;

namespace server.Services.Implementations;

public class GiftService : IGiftService
{
    private readonly IGiftRepository _giftRepository;
    private readonly ILogger<GiftService> _logger;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;
    private readonly IConfiguration _config;

    public GiftService(IGiftRepository giftRepository, ILogger<GiftService> logger, IMapper mapper, IDistributedCache cache,
    IConfiguration config)
    {
        _giftRepository = giftRepository;
        _logger = logger;
        _mapper = mapper;
        _cache = cache;
        _config = config;
    }
    

    // public async Task<IEnumerable<GiftResponseDto>> GetAllGiftsAsync(
    //  PriceSort sort,
    //  int? categoryId,
    //  int? donorId
    //  )
    // {
    //     var gifts = await _giftRepository.GetAllGiftsAsync(
    //         categoryId,
    //         donorId,
    //         sort
    //     );

    //     return _mapper.Map<IEnumerable<GiftResponseDto>>(gifts);

    // }


public async Task<IEnumerable<GiftResponseDto>> GetAllGiftsAsync(
    PriceSort sort,
    int? categoryId,
    int? donorId)
{
    var cacheKey = $"raffle:gifts:{sort}:{categoryId?.ToString() ?? "all"}:{donorId?.ToString() ?? "all"}";

    var cachedJson = await _cache.GetStringAsync(cacheKey);
    if (!string.IsNullOrEmpty(cachedJson))
    {
        var cachedResult = JsonSerializer.Deserialize<IEnumerable<GiftResponseDto>>(cachedJson);
        if (cachedResult != null)
        {
            return cachedResult;
        }
    }

    var gifts = await _giftRepository.GetAllGiftsAsync(
        categoryId,
        donorId,
        sort);

    var result = _mapper.Map<IEnumerable<GiftResponseDto>>(gifts);

    var ttlSeconds = _config.GetValue<int?>("Cache:GiftCacheTtlSeconds") ?? 300;
    var cacheOptions = new DistributedCacheEntryOptions
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(ttlSeconds)
    };

    var payload = JsonSerializer.Serialize(result);
    await _cache.SetStringAsync(cacheKey, payload, cacheOptions);

    return result;
}



    public async Task<IEnumerable<GiftResponseDto>> GetAllAsync(PriceSort sort)
    {
        var gifts = await _giftRepository.GetGiftsAsync(sort);



        return _mapper.Map<IEnumerable<GiftResponseDto>>(gifts);

    }


    public async Task<IEnumerable<GiftResponseDto?>> GetByGiftByCategoryAsync(int categoryId)
    {
        var gifts = await _giftRepository.GetGiftByCategoryAsync(categoryId);
        if (gifts == null) throw new KeyNotFoundException($"No gifts found for category {categoryId}");

        return _mapper.Map<IEnumerable<GiftResponseDto>>(gifts);

    }

    public async Task<IEnumerable<GiftResponseDto>> GetByDonorAsync(int donorId)
    {
        var gifts = await _giftRepository.GetByDonorAsync(donorId);

        return _mapper.Map<IEnumerable<GiftResponseDto>>(gifts);
    }

    public async Task<GiftResponseDto?> GetGiftByIdAsync(int id)
    {
        var gift = await _giftRepository.GetGiftByIdAsync(id);
        if (gift == null)
            throw new KeyNotFoundException($"Gift {id} not found");

        return _mapper.Map<GiftResponseDto>(gift);

    }

    public async Task<GiftResponseDto> AddGiftAsync(GiftCreateWithImageDto dto)
    {

        var exists = await _giftRepository.ExistsByDescriptionAsync(dto.Description);
        if (exists)
            throw new InvalidOperationException("Gift description must be unique");

        var imageUrl = string.Empty;
        if (dto.Image != null)
        {
            // 1. ולידציה בסיסית
            if (dto.Image == null || dto.Image.Length == 0)
                throw new Exception("Image is required");

            // 2. יצירת שם ייחודי
            var extension = Path.GetExtension(dto.Image.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";

            // 3. נתיב פיזי
            var folderPath = Path.Combine("wwwroot", "uploads", "gifts");
            Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            // 4. שמירה בפועל
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }

            // 5. יצירת URL
            imageUrl = $"/uploads/gifts/{fileName}";
        }

        var model = new GiftModel
        {
            Description = dto.Description,
            CategoryId = dto.CategoryId,
            Price = dto.Price,
            DonorId = dto.DonorId,
            ImageUrl = imageUrl
        };

        var created = await _giftRepository.AddGiftAsync(model);

        return _mapper.Map<GiftResponseDto>(created);

    }


    public async Task<GiftResponseDto> UpdateGiftAsync(int id, GiftUpdateWithImageDto dto)
    {
        var existing = await _giftRepository.GetGiftByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Gift {id} not found");

        if (dto.Description != null)
            existing.Description = dto.Description;

        if (dto.Price.HasValue)
            existing.Price = dto.Price.Value;

        if (dto.CategoryId.HasValue)
            existing.CategoryId = dto.CategoryId.Value;

        if (dto.DonorId.HasValue)
            existing.DonorId = dto.DonorId.Value;

        if (dto.HasWinning.HasValue)
            existing.HasWinning = dto.HasWinning.Value;


        if (dto.Image != null)
        {
            // 1. ולידציה בסיסית
            if (dto.Image == null || dto.Image.Length == 0)
                throw new Exception("Image is required");

            // 2. יצירת שם ייחודי
            var extension = Path.GetExtension(dto.Image.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";

            // 3. נתיב פיזי
            var folderPath = Path.Combine("wwwroot", "uploads", "gifts");
            Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            // 4. שמירה בפועל
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }

            // 5. יצירת URL
            existing.ImageUrl = $"/uploads/gifts/{fileName}";
        }

        var updated = await _giftRepository.UpdateGiftAsync(existing);

        if (updated == null)
            throw new KeyNotFoundException($"Gift {id} not found");
        //_logger.LogInformation($"Gift {id} updated successfully. hasWinning: {updated.HasWinning}");
        return _mapper.Map<GiftResponseDto>(updated);

    }

    public async Task MarkGiftAsHavingWinningAsync(int giftId)
    {
        var gift = await _giftRepository.GetGiftByIdAsync(giftId);
        if (gift == null)
            throw new KeyNotFoundException($"Gift {giftId} not found");

        gift.HasWinning = true;
        await _giftRepository.UpdateGiftAsync(gift);
    }


    public async Task<bool> DeleteGiftAsync(int id)
    {
        if (await _giftRepository.HasPurchasesAsync(id))
            throw new InvalidOperationException($"Cannot delete Gift {id} because it has associated purchases.");
        var result = await _giftRepository.DeleteGiftAsync(id);
        if (!result)
            throw new KeyNotFoundException($"Gift {id} not found");
        return result;
    }

    public async Task<IEnumerable<GiftResponseDto>> FilterByGiftName(string name)
    {
        var gifts = await _giftRepository.FilterByGiftName(name);

        return _mapper.Map<IEnumerable<GiftResponseDto>>(gifts);

    }

    public async Task<IEnumerable<GiftResponseDto>> FilterByGiftDonor(string name)
    {
        var gifts = await _giftRepository.FilterByGiftDonor(name);

        return _mapper.Map<IEnumerable<GiftResponseDto>>(gifts);

    }

    public async Task<IEnumerable<GiftPurchaseCountDto>> GetPurchaseCountByGiftAsync()
    {
        var result = await _giftRepository.GetPurchaseCountByGiftAsync();

        return result.Select(g => new GiftPurchaseCountDto
        {
            GiftId = g.GiftId,
            GiftName = g.GiftName,
            PurchaseCount = g.PurchaseCount,
            DonorName = g.DonorName
        });
    }


}