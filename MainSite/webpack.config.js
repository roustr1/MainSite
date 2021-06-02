//const variables
const appBasePath = './Scripts/Components/'; // where the source files located
const publicPath = '../bundle/'; // public path to modify asset urls. eg: '../bundle' => 'www.example.com/bundle/main.js'
const bundleExportPath = './wwwroot/bundle/'; // directory to export build files
const isDev = process.env.NODE_ENV === 'development'
const isProd = !isDev

//import plugins
const path = require('path')
const fs = require('fs');
const { CleanWebpackPlugin } = require('clean-webpack-plugin')
const MiniCssExtractPlugin = require('mini-css-extract-plugin')
const CssMinimizerPlugin = require('css-minimizer-webpack-plugin')
const TerserPlugin = require('terser-webpack-plugin')
const { VueLoaderPlugin } = require('vue-loader')
//const UglifyJsPlugin = require('uglifyjs-webpack-plugin')

// Get entries
const getEntries = function () {
    let jsEntries = {}; // listing to compile

    // We search for js files inside basePath folder and make those as entries
    fs.readdirSync(appBasePath).forEach(function (name) {

        // assumption: modules are located in separate directory and each module component is imported to index.js of particular module
        let indexFile = appBasePath + name + '/index.js'
        if (fs.existsSync(indexFile)) {
            jsEntries[name] = indexFile
        }
    });

    return jsEntries
}


//get modul Optimization
const optimization = function () {
    const config = {}

    if (isProd) {
        // Возможность избавиться от дублирования в финальных bundle файлах
        //(например import библиотеки, использующаяся в двух разных файлах)
        //решить проблему с наименованием файла чанка!!!!!!
        config.splitChunks = {
            chunks: 'all'
        }
        
        config.minimize = true
        config.minimizer = [
            new TerserPlugin(),
            new CssMinimizerPlugin()
        ]
    }
    return config
}

module.exports = {
    //context: path.resolve(__dirname, appBasePath),
    mode: 'development',
    entry: getEntries(),
    output: {
        filename: '[name].js',
        path: path.resolve(__dirname, bundleExportPath),
        publicPath: publicPath
    },
    resolve: {
        //Расширения файлов восприятия webpack-ом по умолчанию (Import)
        extensions: ['.js', '.json', '.vue', '.ts', '.tsx'],
        // Позволяет избавиться от большой вложенности пути до нужного файла(сокращение пути до перехода к нужному файлу)
        // Ключ - сокращенное название, значение - путь до нужного каталога
        alias: {
            '@components': path.resolve(__dirname, appBasePath),
            '@': path.resolve(__dirname, './Scripts/'),
            'vue$': 'vue/dist/vue.esm.js',
        }
    },
    optimization: optimization(),
    plugins: [
        new CleanWebpackPlugin(),
        new VueLoaderPlugin(),
        new MiniCssExtractPlugin({
            filename: '[name].css'
        })
    ],
    module: {
        rules: [
            {
                test: /\.vue$/,
                loader: 'vue-loader',
                options: {
                    loaders: {
                        scss: 'vue-style-loader!css-loader!sass-loader', // <style lang="scss">
                        sass: 'vue-style-loader!css-loader!sass-loader?indentedSyntax' // <style lang="sass">
                    }
                }
            },
            {
                test: /\.s[ac]ss$/,
                use: [
                    {
                        loader: MiniCssExtractPlugin.loader,
                        options: {
                            esModule: true,
                        },
                    },
                    'css-loader',
                    'sass-loader'
                ]
            },
            {
                test: /\.css$/,
                use: [{
                    loader: MiniCssExtractPlugin.loader,
                    options: {
                        esModule: true,
                    },
                }, 'css-loader']
            },
            {
                test: /\.(png|jpe?g|gif|svg)(\?\S*)?$/,
                use: ['file-loader']
            },
            {
                test: /\.(eot|svg|ttf|woff|woff2)(\?\S*)?$/,
                use: 'file-loader'
            },
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: [
                    {
                        loader: 'babel-loader',
                    }
                ]
            }
            
        ]
    },
    devtool: 'source-map', //'#eval-source-map'*/
    /*resolve: {
        extensions: ['.js', '.vue', '.json'],
        alias: {
            'vue$': 'vue/dist/vue.esm.js',
            '@': path.join(__dirname, appBasePath)
        }
    },
    module: {
        rules: [
            {
                test: /\.vue$/,
                loader: 'vue-loader',
                options: {
                    loaders: {
                        scss: 'vue-style-loader!css-loader!sass-loader', // <style lang="scss">
                        sass: 'vue-style-loader!css-loader!sass-loader?indentedSyntax' // <style lang="sass">
                    }
                }
            },
            {
                test: /\.scss$/,
                loader: 'style-loader!css-loader!sass-loader'
            },
            {
                test: /\.css$/,
                loader: 'style-loader!css-loader'
            },
            {
                test: /\.(eot|svg|ttf|woff|woff2)(\?\S*)?$/,
                loader: 'file-loader'
            },
            {
                test: /\.(png|jpe?g|gif|svg)(\?\S*)?$/,
                loader: 'file-loader',
                query: {
                    name: '[name].[ext]?[hash]'
                }
            },
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: {
                    loader: "babel-loader"
                },
            }
        ],
    },
    devtool: '#source-map', //'#eval-source-map'*/
}
/*module.exports.watch = process.env.WATCH === "true";
if (process.env.NODE_ENV === 'production') {
    module.exports.devtool = '#source-map'
    // http://vue-loader.vuejs.org/en/workflow/production.html
    module.exports.plugins = (module.exports.plugins || []).concat([
        new webpack.DefinePlugin({
            'process.env': {
                NODE_ENV: '"production"'
            }
        }),
        new UglifyJsPlugin({
            "uglifyOptions":
            {
                compress: {
                    warnings: false
                },
                sourceMap: true
            }
        }),
    ]);
}
else if (process.env.NODE_ENV === "dev") {
    module.exports.watch = true;
    module.exports.plugins = (module.exports.plugins || []).concat([
        new webpack.DefinePlugin({
            'process.env': {
                NODE_ENV: '"development"'
            }
        }),
    ]);
}*/