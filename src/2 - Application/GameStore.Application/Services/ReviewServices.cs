using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.Application.Commands;
using GameStore.Application.DTOS.Reviews;
using GameStore.Application.Interfaces;
using GameStore.Application.ViewModels;
using GameStore.Domain.Entities;
using GameStore.Domain.Interfaces.Repositories;

namespace GameStore.Application.Services
{
    public class ReviewServices : IReviewServices
    {
        private IUnitOfWork _unit;
        private IMapper _mapper;

        public ReviewServices(
            IUnitOfWork unit,
            IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public IEnumerable<ReviewListViewModel> GetReviewByProductId(Guid productId) {
            return _mapper.Map<IEnumerable<ReviewListViewModel>>(_unit.Reviews.GetReviewByProductId(productId));
        }

        public IEnumerable<ReviewListViewModel> GetReviewByUserId(Guid userId) {
            return _mapper.Map<IEnumerable<ReviewListViewModel>>(_unit.Reviews.GetReviewByUserId(userId));
        }

        public void Delete(Guid id)
        {
            _unit.Reviews.Remove(id);
        }

        public Guid Save(AddOrUpdateReviewDTO command)
        {
            return _unit.Reviews.Add(_mapper.Map<Review>(command));
        }

        public void Update(AddOrUpdateReviewDTO command)
        {
            _unit.Reviews.Update(_mapper.Map<Review>(command));
        }
    }
}