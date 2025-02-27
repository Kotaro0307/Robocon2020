#ifndef QRK_CONNECTION_INFORMATION_H
#define QRK_CONNECTION_INFORMATION_H

/*!
  \file
  \brief 接続情報の管理
  \author Satofumi KAMIMURA

  $Id$
*/

#include "Urg_driver.h"
#include <memory>

namespace qrk
{
    class Connection_information
    {
    public:
        Connection_information(int argc, const char*const argv[], std::string port);
        ~Connection_information(void);

        Lidar::connection_type_t connection_type() const;
        const char* device_or_ip_name(void) const;
        long baudrate_or_port_number(void) const;

    private:
        Connection_information(void);
        struct pImpl;
        std::auto_ptr<pImpl> pimpl;
    };
}

#endif /* !QRK_CONNECTION_INFORMATION_H */
