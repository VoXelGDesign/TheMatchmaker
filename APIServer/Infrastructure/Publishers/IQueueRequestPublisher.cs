using Infrastructure.Publishers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;
public interface IQueueRequestPublisher
{
    public Task PublishAsync(QueueRequest request);
}

