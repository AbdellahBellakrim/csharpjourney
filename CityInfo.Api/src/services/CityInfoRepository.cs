
using CityInfo.Api.src.dbContexts;
using CityInfo.Api.src.entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.Api.src.services;

public class CityInfoRepository : ICityInfoRepository
{
    private readonly CityInfoContext _context;
    public CityInfoRepository(CityInfoContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

    }
    public async Task<IEnumerable<City>> GetCitiesAsync()
    {
        return await _context.Cities.OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<(IEnumerable<City>, PaginationMetaData)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize)
    {
        // collection to start from
        var collection = _context.Cities as IQueryable<City>;
        if (!string.IsNullOrEmpty(name))
        {
            name = name.Trim();
            collection = collection.Where(c => c.Name == name);
        }
        if (!string.IsNullOrEmpty(searchQuery))
        {
            searchQuery = searchQuery.Trim();
            collection = collection.Where(c => c.Name.Contains(searchQuery) || (c.Description != null && c.Description.Contains(searchQuery)));
        }
        var totalItemCount = await collection.CountAsync();
        var paginationMetaData = new PaginationMetaData(totalItemCount, pageSize, pageNumber);
        var collectionToReturn = await collection.OrderBy(c => c.Name).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToListAsync();
        return (collectionToReturn, paginationMetaData);
    }
    public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest)
    {
        if (includePointsOfInterest)
        {
            return await _context.Cities.Include(c => c.PointsOfInterest).Where(c => c.Id == cityId).FirstOrDefaultAsync();
        }
        return await _context.Cities.Where(c => c.Id == cityId).FirstOrDefaultAsync();

    }

    public async Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int pointOfInterestId)
    {
        return await _context.PointsOfInterest.Where(p => p.CityId == cityId && p.Id == pointOfInterestId).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<PointOfInterest>> GetPointOfInterestsAsync(int cityId)
    {
        return await _context.PointsOfInterest.Where(p => p.CityId == cityId).ToListAsync();
    }

    public async Task<bool> CityExistsAsync(int cityId)
    {
        return await _context.Cities.AnyAsync(c => c.Id == cityId);
    }

    public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
    {
        var city = await GetCityAsync(cityId, false);
        if (city != null)
        {
            city.PointsOfInterest.Add(pointOfInterest);
        }
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }

    public void DeletePointOfInterest(PointOfInterest pointOfInterest)
    {
        _context.PointsOfInterest.Remove(pointOfInterest);
    }

    public async Task<bool> CityNameMatchesId(int cityId, string? cityName)
    {
        return await _context.Cities.AnyAsync(c => c.Id == cityId && c.Name == cityName);
    }
}