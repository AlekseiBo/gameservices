using System;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServices
{
    public class RequestQueue
    {
        ServiceRateLimiter m_QueryCooldown = new ServiceRateLimiter(1, 1f);
        ServiceRateLimiter m_CreateCooldown = new ServiceRateLimiter(2, 6f);
        ServiceRateLimiter m_JoinCooldown = new ServiceRateLimiter(2, 6f);
        ServiceRateLimiter m_QuickJoinCooldown = new ServiceRateLimiter(1, 10f);
        ServiceRateLimiter m_GetLobbyCooldown = new ServiceRateLimiter(1, 1f);
        ServiceRateLimiter m_DeleteLobbyCooldown = new ServiceRateLimiter(2, 1f);
        ServiceRateLimiter m_UpdateLobbyCooldown = new ServiceRateLimiter(5, 5f);
        ServiceRateLimiter m_UpdatePlayerCooldown = new ServiceRateLimiter(5, 5f);
        ServiceRateLimiter m_LeaveLobbyOrRemovePlayer = new ServiceRateLimiter(5, 1);
        ServiceRateLimiter m_HeartBeatCooldown = new ServiceRateLimiter(5, 30);

        public class ServiceRateLimiter
        {
            public Action<bool> onCooldownChange;
            public readonly int coolDownMS;
            public bool TaskQueued { get; private set; } = false;

            readonly int m_ServiceCallTimes;
            bool m_CoolingDown = false;
            int m_TaskCounter;

            //(If you're still getting rate limit errors, try increasing the pingBuffer)
            public ServiceRateLimiter(int callTimes, float coolDown, int pingBuffer = 100)
            {
                m_ServiceCallTimes = callTimes;
                m_TaskCounter = m_ServiceCallTimes;
                coolDownMS =
                    Mathf.CeilToInt(coolDown * 1000) +
                    pingBuffer;
            }

            public async Task QueueUntilCooldown()
            {
                if (!m_CoolingDown)
                {
#pragma warning disable 4014
                    ParallelCooldownAsync();
#pragma warning restore 4014
                }

                m_TaskCounter--;

                if (m_TaskCounter > 0)
                {
                    return;
                }

                if (!TaskQueued)
                    TaskQueued = true;
                else
                    return;

                while (m_CoolingDown)
                {
                    await Task.Delay(10);
                }
            }

            async Task ParallelCooldownAsync()
            {
                IsCoolingDown = true;
                await Task.Delay(coolDownMS);
                IsCoolingDown = false;
                TaskQueued = false;
                m_TaskCounter = m_ServiceCallTimes;
            }

            public bool IsCoolingDown
            {
                get => m_CoolingDown;
                private set
                {
                    if (m_CoolingDown != value)
                    {
                        m_CoolingDown = value;
                        onCooldownChange?.Invoke(m_CoolingDown);
                    }
                }
            }
        }
    }
}