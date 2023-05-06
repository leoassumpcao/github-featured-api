using AutoMapper;
using GithubFeatured.Application.Models;
using GithubFeatured.Infra.Services.GitHub.Models;
using GitHubFeatured.Domain.Entities;

namespace GithubFeatured.Application.Mapping
{
    public class RepoProfile : Profile
    {
        public RepoProfile()
        {
            CreateMap<Repo, RepoModel>()
                .ReverseMap();

            CreateMap<Repo, GithubRepoModel>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(f => f.GitHubId))
            .ReverseMap()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(f => Guid.NewGuid()))
                .ForMember(
                    dest => dest.OwnerUser,
                    opt => opt.MapFrom(f => f.Owner.Login));
        }
    }
}
