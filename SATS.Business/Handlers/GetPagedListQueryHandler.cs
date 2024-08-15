//using Ardalis.Specification;
//using AutoMapper;
//using MediatR;
//using SATS.Business.Queries;

//namespace SATS.Business.Handlers
//{
//    public class GetPagedListQueryHandler<TDto, TEntity> : IRequestHandler<GetPagedListQuery<TDto, TEntity>, PagedList<TDto>>
//        where TEntity : class
//    {
//        private readonly IRepositoryBase<TEntity> _repository;
//        private readonly IMapper _mapper;

//        public GetPagedListQueryHandler(IRepositoryBase<TEntity> repository, IMapper mapper)
//        {
//            _repository = repository;
//            _mapper = mapper;
//        }

//        public async Task<PagedList<TDto>> Handle(GetPagedListQuery<TDto, TEntity> request, CancellationToken cancellationToken)
//        {
//            var spec = new PagedSpecification<TEntity>(request.PageIndex, request.PageSize);
//            var totalCount = await _repository.CountAsync(spec, cancellationToken);
//            var entities = await _repository.ListAsync(spec, cancellationToken);
//            var dtos = _mapper.Map<List<TDto>>(entities);

//            return new PagedList<TDto>
//            {
//                Items = dtos,
//                TotalCount = totalCount,
//                PageIndex = request.PageIndex,
//                PageSize = request.PageSize
//            };
//        }
//    }
//}


//namespace SATS.Business.Queries
//{
//    public class GetPagedListQuery<TDto, TEntity> : IRequest<PagedList<TDto>>
//    {
//        public int PageIndex { get; }
//        public int PageSize { get; }

//        public GetPagedListQuery(int pageIndex, int pageSize)
//        {
//            PageIndex = pageIndex;
//            PageSize = pageSize;
//        }
//    }

//    public class PagedList<T>
//    {
//        public List<T> Items { get; set; }
//        public int TotalCount { get; set; }
//        public int PageIndex { get; set; }
//        public int PageSize { get; set; }
//    }
//    public class PagedSpecification<T> : Specification<T>
//    {
//        public PagedSpecification(int pageIndex, int pageSize)
//        {
//            Query
//                .Skip((pageIndex - 1) * pageSize)
//                .Take(pageSize);
//        }
//    }

//}
