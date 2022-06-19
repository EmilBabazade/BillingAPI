﻿using BillingAPI.Entities;

namespace BillingAPI.Data.interfaces
{
    public interface IGatewayRepository
    {
        public void Add(Gateway gateway);
        public Task Update(Gateway gateway);
        public Task<IEnumerable<Gateway>> GetAll(string order = "");
        public Task<Gateway> GetById(int id);
        public void DeleteById(int id);
    }
}
